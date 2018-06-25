using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestCedro.Domain.Entities;
namespace TestCedro.Domain.Interfaces.Services
{
    public interface IRestaurantService : IDisposable
    {
        Restaurant Add(Restaurant obj);
        Restaurant Update(Restaurant obj);
        void Remove(Guid id);
        Restaurant GetById(Guid id);
        IEnumerable<Restaurant> Search(Expression<Func<Restaurant, bool>> predicate);
        IEnumerable<Restaurant> GetPagination(int skip, int take, string search);
    }
}