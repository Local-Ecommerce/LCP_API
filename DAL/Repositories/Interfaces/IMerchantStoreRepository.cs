using System.Collections.Generic;
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
        /// <param name="residentId"></param>
        /// <param name="status"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<MerchantStore>> GetMerchantStore(
            string id, string apartmentId, string residentId,
            int?[] status, string search, int? limit,
            int? queryPage, bool isAsc,
            string propertyName, string[] include);

        /// <summary>
        /// Get Merchant Stores By Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<List<MerchantStore>> GetMerchantStoresByIdsAndApartmentId(List<string> ids, string apartmentId);
    }
}