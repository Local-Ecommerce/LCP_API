using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RepositoryLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //private readonly Local4YouContext _context;
        private readonly DbSet<T> _dbSet;

        //public Repository(Local4YouContext context)
        //{
        //    _context = context;
        //    _dbSet = _context.Set<T>();
        //}

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Get(string id)
        {
            return _dbSet.Find(id);
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
