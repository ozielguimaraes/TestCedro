using System;
using System.Linq;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Helpers;

namespace TestCedro.Infra.Data.Context
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            using (MainContext context = new MainContext())
            {
                context.Database.EnsureCreated();

                if (context.Users.Any(x => x.Email == "contato@ozielguimaraes.net")) return;

                var users = new[]
                {
                    new User
                    {
                        FirstName = "User",
                        LastName = "Client",
                        Email = "contato@ozielguimaraes.net",
                        IsApproved = true,
                        PasswordFailuresSinceLastSuccess = 0,
                        LastPasswordFailureDate = null,
                        LastActivityDate = null,
                        LastLockoutDate = null,
                        LastLoginDate = null,
                        ConfirmationToken = null,
                        CreationDate = DateTime.Now,
                        IsLockedOut = false,
                        LastPasswordChangedDate = null,
                        PasswordVerificationToken = null,
                        PrivateKey = null,
                        PasswordVerificationTokenExpirationDate = null,
                        PictureUrl = null,
                        Comment = null,
                        Password = PasswordAssertionConcern.ComputeHash("123456")
                    }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}