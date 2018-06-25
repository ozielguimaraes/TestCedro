using System.Collections.Generic;
using TestCedro.Domain.Entities;
namespace TestCedro.Domain.Interfaces.Repositories
{
    public interface IDishRepository : IRepository<Dish>
    {
        IEnumerable<Dish> GetPagination(int skip, int take, string search);
    }
}