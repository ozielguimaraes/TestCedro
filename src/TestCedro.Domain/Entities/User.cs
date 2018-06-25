using System;

namespace TestCedro.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public DateTime? LastLockoutDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string ConfirmationToken { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public string PasswordVerificationToken { get; set; }
        public string PrivateKey { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
        public string PictureUrl { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public static int TokenSizeInBytes => 16;
        public static int MaxInvalidPasswordAttempts => 5;
        public static int MinRequiredNonAlphanumericCharacters => 0;
        public static int MinRequiredPasswordLength => 6;
        public static int PasswordAttemptWindow => 10;
        public static int PasswordAnswerAttemptLockoutDuration => 5;
        public static bool RequiresUniqueEmail => true;
        public bool FirstAccess() => LastPasswordChangedDate == CreationDate;

        internal void Update(User obj)
        {
            Email = obj.Email;
            FirstName = obj.Email;
            LastName = obj.LastName;
            Comment = obj.Comment;
        }
    }
}