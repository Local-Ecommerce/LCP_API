using System.Threading.Tasks;
using BLL.Dtos;
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
        Task<BaseResponse<ExtendStoreMenuDetailResponse>> CreateStoreMenuDetail(StoreMenuDetailRequest request);


        /// <summary>
        /// Create Default Store Menu Detail
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        ExtendStoreMenuDetailResponse CreateDefaultStoreMenuDetail(string menuId, string merchantStoreId);


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendStoreMenuDetailResponse>> UpdateStoreMenuDetailById(string id, StoreMenuDetailUpdateRequest request);


        Task<BaseResponse<ExtendStoreMenuDetailResponse>> DeleteStoreMenuDetailById(string id);
    }
}