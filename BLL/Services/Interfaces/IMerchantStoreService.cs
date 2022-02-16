﻿using BLL.Dtos.MerchantStore;
using BLL.Dtos.StoreMenuDetail;
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
        Task<MerchantStoreResponse> CreateMerchantStore(MerchantStoreRequest merchantStoreRequest);


        /// <summary>
        /// Get Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExtendMerchantStoreResponse> GetMerchantStoreById(string id);


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
        /// Get Merchant By Account Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<List<MerchantStoreResponse>> GetMerchantStoreByApartmentId(string apartmentId);


        /// <summary>
        /// Add Store Menu Details To Merchant Store
        /// </summary>
        /// <param name="storeMenuDetailRequest"></param>
        /// <returns></returns>
        Task<List<StoreMenuDetailResponse>> AddStoreMenuDetailsToMerchantStore(string merchantStoreId,
            List<StoreMenuDetailRequest> storeMenuDetailRequest);


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <returns></returns>
        Task<StoreMenuDetailResponse> UpdateStoreMenuDetailById(string storeMenuDetailId,
            StoreMenuDetailUpdateRequest storeMenuDetailUpdateRequest);


        /// <summary>
        /// Delete Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <returns></returns>
        Task<StoreMenuDetailResponse> DeleteStoreMenuDetailById(string storeMenuDetailId);


        /// <summary>
        /// Get Merchant Stores By Status
        /// </summary>
        /// <returns></returns>
        Task<List<MerchantStoreResponse>> GetMerchantStoresByStatus(int status);


        /// <summary>
        /// Get All Merchant Stores
        /// </summary>
        /// <returns></returns>
        Task<List<ExtendMerchantStoreResponse>> GetAllMerchantStores();


        /// <summary>
        /// Get Pending Merchant Stores
        /// </summary>
        /// <returns></returns>
        Task<List<ExtendMerchantStoreResponse>> GetPendingMerchantStores();


        /// <summary>
        /// Get Menus By Store Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExtendMerchantStoreResponse> GetMenusByStoreId(string id);


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
