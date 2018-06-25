    using System;
using System.Linq;
using System.Threading.Tasks;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Repositories;
using TestCedro.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace TestCedro.Infra.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MainContext context) : base(context) { }

        public Guid GetUserIdFromPasswordResetToken(string token)
        {
            var user = DbSet.FirstOrDefault(x => x.PasswordVerificationToken == token);
            return user != null ? user.UserId : Guid.Empty;
        }

        public User GetUser(string email)
        {
            return DbSet.FirstOrDefault(x => x.Email == email);
        }

        public Guid GetUserIdFromAccountConfirmationToken(string token)
        {
            var user = DbSet.FirstOrDefault(x => x.ConfirmationToken == token);
            return user != null ? user.UserId : Guid.Empty;
        }

        public bool EmailExists(string email)
        {
            return DbSet.Any(x => x.Email == email);
        }

        public Task<User> GetByUserEmailAsync(string email)
        {
            return DbSet.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}