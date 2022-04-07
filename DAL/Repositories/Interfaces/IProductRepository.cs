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
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<PagingModel<Product>> GetProduct(
            string id = default, int?[] status = default, string apartmentId = default, string categoryId = default,
            string search = default, int? limit = default, int? queryPage = default,
            bool isAsc = default, string propertyName = default, string[] include = default, string residentId = default);
    }
}