using DAL.Models;
using System;
using DAL.Repositories;
using System.Collections.Generic;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoichDBContext _context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(LoichDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Commit Unit of Work
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get Repository of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)repositories[type];
        }
    }
}
