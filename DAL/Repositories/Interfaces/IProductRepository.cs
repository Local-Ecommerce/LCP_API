using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="apartmentId"></param>
        /// <param name="categoryId"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<Product>> GetProduct(
            string id, int?[] status, string apartmentId, string categoryId,
            int? limit, int? queryPage,
            bool isAsc, string propertyName, string[] include);
    }
}