using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Dtos.StoreMenuDetail;

namespace BLL.Services.Interfaces
{
    public interface IStoreMenuDetailService
    {
        /// <summary>
        /// Add Store Menu Details To Merchant Store
        /// </summary>
        /// <param name="storeMenuDetailRequest"></param>
        /// <returns></returns>
        Task<List<StoreMenuDetailResponse>> AddStoreMenuDetailsToMerchantStore(string merchantStoreId,
            List<StoreMenuDetailRequest> storeMenuDetailRequest);


        /// <summary>
        /// Create Default Store Menu Detail
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        StoreMenuDetailResponse CreateDefaultStoreMenuDetail(string menuId, string merchantStoreId);


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<StoreMenuDetailResponse> UpdateStoreMenuDetailById(string id, StoreMenuDetailUpdateRequest request);


        /// <summary>
        /// Delete Store Menu Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StoreMenuDetailResponse> DeleteStoreMenuDetailById(string id);
    }
}