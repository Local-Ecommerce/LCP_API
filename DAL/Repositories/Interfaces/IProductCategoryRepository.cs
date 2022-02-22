using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        /// <summary>
        /// Get ProductCategory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Task<PagingModel<ProductCategory>> GetProductCategory(
            string id, int?[] status,
            int? limit, int? queryPage,
            bool isAsc, string propertyName);
    }
}