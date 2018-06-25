using System.Collections.Generic;
using System.Linq;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Repositories;
using TestCedro.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace TestCedro.Infra.Data.Repositories
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        public DishRepository(MainContext context) : base(context) { }

        public IEnumerable<Dish> GetPagination(int skip, int take, string search)
        {
            return DbSet
                .Where(x => x.Name.ToLower().Contains(search))
                .OrderBy(x => x.Name)
                .Skip((skip - 1) * take)
                .Take(take)
                .Include(x => x.Restaurant);
        }
    }
}