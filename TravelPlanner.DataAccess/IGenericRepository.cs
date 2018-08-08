using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TravelPlanner.DataAccess
{
    public interface IGenericRepository
    {
        Task<T> Find<T>(Expression<Func<T, bool>> predicate) where T: class;
        Task<IEnumerable<T>> GetList<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<IEnumerable<T>> GetAll<T>() where T : class;
        void Remove<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        Task SaveChanges();
        void Dispose();
    }
}