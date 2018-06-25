using System.Collections.Generic;
using System.Linq;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Repositories;
using TestCedro.Infra.Data.Context;

namespace TestCedro.Infra.Data.Repositories
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(MainContext context) : base(context) { }

        public IEnumerable<Restaurant> GetPagination(int skip, int take, string search)
        {
            return DbSet
                .Where(x => x.Name.ToLower().Contains(search))
                .OrderBy(x => x.Name)
                .Skip((skip - 1) * take)
                .Take(take);
        }
    }
}