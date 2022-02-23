using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IProductCombinationRepository : IRepository<ProductCombination>
    {
        /// <summary>
        /// Get Product Combination
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Task<PagingModel<ProductCombination>> GetProductCombination(
            string id, string productId,
            int?[] status,
            int? limit, int? queryPage,
            bool isAsc, string propertyName);
    }
}