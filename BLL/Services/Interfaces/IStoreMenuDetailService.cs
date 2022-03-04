using System.Threading.Tasks;
using BLL.Dtos.StoreMenuDetail;

namespace BLL.Services.Interfaces
{
    public interface IStoreMenuDetailService
    {
        /// <summary>
        /// Create Store Menu Details
        /// </summary>
        /// <param name="storeMenuDetailRequest"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<StoreMenuDetailResponse> CreateStoreMenuDetails(
            StoreMenuDetailRequest storeMenuDetailRequest, string menuId);


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