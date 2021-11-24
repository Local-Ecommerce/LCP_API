using DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;

        Task SaveChangesAsync();
    }
}
