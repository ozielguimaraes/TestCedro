using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces;
using TestCedro.Domain.Interfaces.Repositories;
using TestCedro.Domain.Interfaces.Services;
namespace TestCedro.Domain.Services
{
    public class DishService : BaseService, IDishService
    {
        private readonly IDishRepository _dishRepository;

        public DishService(IUnitOfWork uow, IDishRepository dishRepository) : base(uow)
        {
            _dishRepository = dishRepository;
        }

        public Dish Add(Dish obj)
        {
            BeginTransaction();
            obj = _dishRepository.Add(obj);
            Commit();
            return obj;
        }

        public Dish Update(Dish obj)
        {
            BeginTransaction();
            var dish = GetById(obj.DishId);
            if (dish == null) throw new ArgumentNullException(nameof(dish));
            dish.Update(obj);
            obj = _dishRepository.Update(dish);
            Commit();
            return obj;
        }

        public void Remove(Guid id)
        {
            BeginTransaction();
            _dishRepository.Remove(GetById(id));
            Commit();
        }

        public Dish GetById(Guid id)
        {
            return _dishRepository.GetById(id);
        }

        public IEnumerable<Dish> Search(Expression<Func<Dish, bool>> predicate)
        {
            return _dishRepository.Search(predicate).OrderBy(o => o.Name);
        }

        public IEnumerable<Dish> GetPagination(int skip, int take, string search)
        {
            return _dishRepository.GetPagination(skip, take, search);
        }

        public void Dispose()
        {
            _dishRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}