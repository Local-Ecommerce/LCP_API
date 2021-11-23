using System.Linq;

namespace DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        T Get(string id);

        T Get(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
