using RepositoryLayer.Repositories;
using System;
using System.Collections.Generic;

namespace RepositoryLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //private readonly Local4YouContext _context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        //public UnitOfWork(Local4YouContext context)
        //{
        //    _context = context;
        //}

        /*
         * [12/08/2021 - HanNQ] commit Unit of Work
         */
        public int Commit()
        {
            //return _context.SaveChanges();
            return default;
        }

        public void Dispose()
        {
            //_context.Dispose();
            GC.SuppressFinalize(this);
        }

        /*
         * [12/08/2021 - HanNQ] get Repository
         */
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                //var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                //repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)repositories[type];
        }
    }
}
