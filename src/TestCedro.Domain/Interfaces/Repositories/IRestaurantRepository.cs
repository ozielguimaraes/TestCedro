using System.Collections.Generic;
using TestCedro.Domain.Entities;
namespace TestCedro.Domain.Interfaces.Repositories
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        IEnumerable<Restaurant> GetPagination(int skip, int take, string search);
    }
}