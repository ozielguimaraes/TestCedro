using System.Threading.Tasks;

namespace TestCedro.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        Task<int> CommitAsync();
    }
}
