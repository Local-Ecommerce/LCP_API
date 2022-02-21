using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IMerchantStoreRepository : IRepository<MerchantStore>
    {
        /// <summary>
        /// Get Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<MerchantStore>> GetMerchantStore(
            string id, string apartmentId,
            int?[] status, int? limit, 
            int? queryPage, bool isAsc, 
            string propertyName, string[] include);

    }
}