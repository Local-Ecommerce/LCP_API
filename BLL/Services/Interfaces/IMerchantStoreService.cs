using BLL.Dtos;
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


        /// <summary>
        /// Get Merchant Store By Store Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BaseResponse<MerchantStoreResponse>> GetMerchantStoreByStoreName(string name);


        /// <summary>
        /// Get Merchant By Account Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoreByMerchantId(string merchantId);


        /// <summary>
        /// Get Merchant By Account Id
        /// </summary>
        /// <param name="appartmentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoreByApartmentId(string apartmentId);
    }
}
