using RepositoryLayer.Repositories;
using System;

namespace RepositoryLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        /*
         * [12/08/2021 - HanNQ] get Repository
         */
        IRepository<T> Repository<T>() where T : class;

        /*
         * [12/08/2021 - HanNQ] commit Unit of Work
         */
        int Commit();
    }
}
