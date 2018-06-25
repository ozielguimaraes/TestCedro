using System;
using System.Threading.Tasks;
using TestCedro.Domain.Entities;
namespace TestCedro.Domain.Interfaces.Services
{
    public interface IUserService : IDisposable
    {
        bool ChangePassword(string email, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(User user, string password);
        string Create(string email, string password, string confirmPassword, string firstName, string lastName, bool requireConfirmationToken = false);
        Task<dynamic> CreateAsync(User appUser, string substring);
        string GeneratePasswordResetToken(string email);
        User GetByUserEmail(string email);
        Task<User> GetByUserEmailAsync(string email);
        bool ResetPassword(string passwordResetToken, string newPassword);
        bool EmailExists(string email);
        bool ConfirmAccount(string accountConfirmationToken);
        Guid GetUserIdFromPasswordResetToken(string token);
        User GetById(Guid id);
        User Update(User obj);
    }
}