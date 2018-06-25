using System;
using System.Threading.Tasks;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Helpers;
using TestCedro.Domain.Interfaces;
using TestCedro.Domain.Interfaces.Repositories;
using TestCedro.Domain.Interfaces.Services;
namespace TestCedro.Domain.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork uow, IUserRepository userRepository) : base(uow)
        {
            _userRepository = userRepository;
        }

        public bool ChangePassword(string email, string currentPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(email)) return false;
            if (string.IsNullOrEmpty(currentPassword)) return false;
            if (string.IsNullOrEmpty(newPassword)) return false;
            var user = GetByUserEmail(email);
            if (user == null) return false;
            var hashedPassword = user.Password;
            var verificationSucceeded = hashedPassword != null && PasswordAssertionConcern.VerifyHash(currentPassword, hashedPassword);
            if (verificationSucceeded)
            {
                user.PasswordFailuresSinceLastSuccess = 0;
            }
            else
            {
                var failures = user.PasswordFailuresSinceLastSuccess;
                if (failures < User.MaxInvalidPasswordAttempts)
                {
                    user.PasswordFailuresSinceLastSuccess += 1;
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                }
                else if (failures >= User.MaxInvalidPasswordAttempts)
                {
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                    user.LastLockoutDate = DateTime.UtcNow;
                    user.IsLockedOut = true;
                }
                BeginTransaction();
                _userRepository.Update(user);
                Commit();
                return false;
            }
            var newHashedPassword = PasswordAssertionConcern.ComputeHash(newPassword, "SHA512", null);
            user.Password = newHashedPassword;
            user.LastPasswordChangedDate = DateTime.UtcNow;
            BeginTransaction();
            _userRepository.Update(user);
            Commit();
            return true;
        }

        private void UnlockUser(User user)
        {
            if (user == null) throw new ArgumentException(nameof(user));
            user.IsLockedOut = false;
            user.PasswordFailuresSinceLastSuccess = 0;
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            //message = string.Empty;
            if (!user.IsApproved) { /*message = "Usuário desativado.";*/ return Task.FromResult(false); }
            if (user.LastLockoutDate.HasValue)
            {
                var timeout = user.LastLockoutDate.Value.AddMinutes(User.PasswordAnswerAttemptLockoutDuration);
                if (user.IsLockedOut && timeout >= DateTime.UtcNow) { /*message = "Usuário bloqueado.";*/ return Task.FromResult(false); }
                if (user.IsLockedOut && timeout < DateTime.UtcNow) UnlockUser(user);
            }

            var verificationSucceeded = user.Password != null && PasswordAssertionConcern.VerifyHash(password, user.Password);
            if (verificationSucceeded)
            {
                user.PasswordFailuresSinceLastSuccess = 0;
                user.LastLoginDate = DateTime.UtcNow;
                user.LastActivityDate = DateTime.UtcNow;
                user.IsLockedOut = false;
            }
            else
            {
                var failures = user.PasswordFailuresSinceLastSuccess;
                if (failures < User.MaxInvalidPasswordAttempts)
                {
                    user.PasswordFailuresSinceLastSuccess += 1;
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                    //message = "O email ou senha está incorreta.";
                }
                else if (failures >= User.MaxInvalidPasswordAttempts)
                {
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                    user.LastLockoutDate = DateTime.UtcNow;
                    user.IsLockedOut = true;
                    //message = "Usuário bloqueado.";
                }
            }
            BeginTransaction();
            _userRepository.Update(user);
            Commit();
            return Task.FromResult(verificationSucceeded);
        }

        public string Create(string email, string password, string confirmPassword, string firstName, string lastName, bool requireConfirmationToken = false)
        {
            if (string.IsNullOrEmpty(email)) throw new Exception("Login inválido");
            if (string.IsNullOrEmpty(password)) throw new Exception("Senha inválida");
            if (!string.IsNullOrEmpty(password) && password.Length < User.MinRequiredPasswordLength) throw new Exception($"A senha deve ter no mínimo {User.MinRequiredPasswordLength} caracteres");
            if (password != confirmPassword) throw new Exception("Senhas não conferem");
            if (User.RequiresUniqueEmail && EmailExists(email)) throw new Exception("Email duplicado");
            var hashedPassword = PasswordAssertionConcern.ComputeHash(password, "SHA512", null);

            var token = string.Empty;
            var privateKey = string.Empty;
            if (requireConfirmationToken)
            {
                var time = DateTime.UtcNow.AddMinutes(Token._expirationMinutes);
                privateKey = Token.GenerateToken($"{email}{Token._TestCedro_PRIVATE_KEY}", time.Ticks);
                token = Token.GenerateToken(email, time.Ticks);
            }
            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Password = hashedPassword,
                IsApproved = !requireConfirmationToken,
                Email = email,
                CreationDate = DateTime.UtcNow,
                LastPasswordChangedDate = DateTime.UtcNow,
                PasswordFailuresSinceLastSuccess = 0,
                LastLoginDate = DateTime.UtcNow,
                LastActivityDate = DateTime.UtcNow,
                LastLockoutDate = DateTime.UtcNow,
                IsLockedOut = false,
                LastPasswordFailureDate = DateTime.UtcNow,
                ConfirmationToken = token,
                PrivateKey = privateKey
            };
            BeginTransaction();
            _userRepository.Add(user);
            Commit();
            return user.ConfirmationToken;
        }

        public string GeneratePasswordResetToken(string email)
        {
            var user = GetByUserEmail(email);
            if (user == null) return string.Empty;
            if (!user.IsApproved) return string.Empty;
            var time = DateTime.UtcNow.AddMinutes(Token._expirationMinutes);
            user.PasswordVerificationTokenExpirationDate = time;
            user.PasswordVerificationToken = Token.GenerateToken(email, time.Ticks);
            user.PrivateKey = Token.GenerateToken($"{email}{Token._TestCedro_PRIVATE_KEY}", time.Ticks);
            BeginTransaction();
            _userRepository.Update(user);
            Commit();
            return user.PasswordVerificationToken;
        }

        public User GetByUserEmail(string email)
        {
            return _userRepository.GetUser(email);
        }

        public Task<User> GetByUserEmailAsync(string email)
        {
            return _userRepository.GetByUserEmailAsync(email);
        }

        public bool ResetPassword(string passwordResetToken, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword)) return false;
            var user = GetById(GetUserIdFromPasswordResetToken(passwordResetToken));
            if (user == null) return false;
            if (!Token.IsTokenValid(passwordResetToken)) throw new Exception("Token inválido");
            if (user.PasswordVerificationToken != passwordResetToken) return false;
            var newHashedPassword = PasswordAssertionConcern.ComputeHash(newPassword, "SHA512", null);
            user.Password = newHashedPassword;
            user.LastPasswordChangedDate = DateTime.UtcNow;
            BeginTransaction();
            _userRepository.Update(user);
            Commit();
            return true;
        }

        public bool EmailExists(string email)
        {
            return _userRepository.EmailExists(email);
        }

        public bool ConfirmAccount(string accountConfirmationToken)
        {
            if (string.IsNullOrEmpty(accountConfirmationToken)) return false;
            var user = GetById(GetUserIdFromAccountConfirmationToken(accountConfirmationToken));
            if (user == null) return false;
            if (user.IsApproved) throw new Exception("Email já confirmado");
            if (!Token.IsTokenValid(accountConfirmationToken)) throw new Exception("Token inválido");
            user.IsApproved = user.ConfirmationToken == accountConfirmationToken;
            BeginTransaction();
            _userRepository.Update(user);
            Commit();
            return user.IsApproved;
        }

        public User GetById(Guid id)
        {
            return id != Guid.Empty ? _userRepository.GetById(id) : null;
        }

        public User Update(User obj)
        {
            BeginTransaction();
            var user = GetById(obj.UserId);
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.Update(obj);
            obj = _userRepository.Update(user);
            Commit();
            return obj;
        }

        public Task<dynamic> CreateAsync(User appUser, string substring)
        {
            throw new NotImplementedException();
        }

        public Guid GetUserIdFromPasswordResetToken(string token)
        {
            return _userRepository.GetUserIdFromPasswordResetToken(token);
        }

        public Guid GetUserIdFromAccountConfirmationToken(string token)
        {
            return _userRepository.GetUserIdFromAccountConfirmationToken(token);
        }

        public void Dispose()
        {
            _userRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}