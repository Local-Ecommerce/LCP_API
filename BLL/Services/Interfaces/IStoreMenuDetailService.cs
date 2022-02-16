using System.Threading.Tasks;
using BLL.Dtos.StoreMenuDetail;

namespace BLL.Services.Interfaces
{
    public interface IStoreMenuDetailService
    {
        /// <summary>
        /// Create Store Menu Detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<StoreMenuDetailResponse> CreateStoreMenuDetail(StoreMenuDetailRequest request);


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