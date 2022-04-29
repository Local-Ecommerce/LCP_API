using BLL.Dtos.MerchantStore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMerchantStoreService
    {
        /// <summary>
        /// Create Merchant Store
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        Task<MerchantStoreResponse> CreateMerchantStore(string residentId, MerchantStoreRequest merchantStoreRequest);


        /// <summary>
        /// Get Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="residentId"></param>
        /// <param name="role"></param>
        /// <param name="status"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetMerchantStores(
            string id, string apartmentId, string residentId,
            string role, int?[] status, string search,
            int? limit, int? page,
            string sort, string[] include);


        /// <summary>
        /// Get Unverified Merchant Stores
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<List<ExtendMerchantStoreResponse>> GetUnverifiedMerchantStores(string residentId);


        /// <summary>
        /// Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateMerchantStoreById(string id, MerchantStoreRequest request, string residentId);


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task DeleteMerchantStore(string id, string residentId);


        /// <summary>
        /// Verify Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isApprove"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<ExtendMerchantStoreResponse> VerifyMerchantStore(string id, bool isApprove, string residentId);


        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residendId"></param>
        /// <returns></returns>
        Task<ExtendMerchantStoreResponse> Warning(string id, string residendId);
    }

}
