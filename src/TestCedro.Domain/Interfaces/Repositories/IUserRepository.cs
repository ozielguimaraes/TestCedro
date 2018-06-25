using System;
using System.Threading.Tasks;
using TestCedro.Domain.Entities;
namespace TestCedro.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Guid GetUserIdFromPasswordResetToken(string token);
        User GetUser(string email);
        Guid GetUserIdFromAccountConfirmationToken(string token);
        bool EmailExists(string email);
        Task<User> GetByUserEmailAsync(string email);
    }
}