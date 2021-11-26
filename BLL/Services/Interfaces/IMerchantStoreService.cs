using BLL.Dtos;
using BLL.Dtos.MerchantStore;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMerchantStoreService
    {
        /// <summary>
        /// Create Merchant Store
        /// </summary>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantStoreResponse>> CreateMerchantStore(MerchantStoreRequest merchantStoreRequest);


        /// <summary>
        /// Get Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantStoreResponse>> GetMerchantStoreById(string id);


        /// <summary>
        /// Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantStoreResponse>> UpdateMerchantStoreById(string id, MerchantStoreRequest merchantStoreRequest);


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantStoreResponse>> DeleteMerchantStore(string id);
    }
}
