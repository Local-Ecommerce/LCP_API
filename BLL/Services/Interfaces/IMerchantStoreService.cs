using BLL.Dtos.MerchantStore;
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
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetMerchantStore(
            string id, string apartmentId, string residentId,
            string role, int?[] status,
            int? limit, int? page,
            string sort, string[] include);


        /// <summary>
        /// Request Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ExtendMerchantStoreResponse> RequestUpdateMerchantStoreById(string id, MerchantStoreUpdateRequest request);


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MerchantStoreResponse> DeleteMerchantStore(string id);


        /// <summary>
        /// Verify Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCreate"></param>
        /// <param name="isApprove"></param>
        /// <returns></returns>
        Task<ExtendMerchantStoreResponse> VerifyMerchantStore(string id, bool isApprove);
    }

}
