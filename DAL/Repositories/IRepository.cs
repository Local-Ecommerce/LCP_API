using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<List<T>> FindListAsync(Expression<Func<T, bool>> expression);

        Task<T> FindAsync(Expression<Func<T, bool>> expression);
    }
}
