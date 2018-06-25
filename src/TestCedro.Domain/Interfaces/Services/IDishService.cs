using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestCedro.Domain.Entities;
namespace TestCedro.Domain.Interfaces.Services
{
    public interface IDishService : IDisposable
    {
        Dish Add(Dish obj);
        Dish Update(Dish obj);
        void Remove(Guid id);
        Dish GetById(Guid id);
        IEnumerable<Dish> Search(Expression<Func<Dish, bool>> predicate);
        IEnumerable<Dish> GetPagination(int skip, int take, string search);
    }
}
