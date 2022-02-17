using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.MerchantStore;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BLL.Dtos.StoreMenuDetail;

namespace BLL.Services
{
    public class MerchantStoreService : IMerchantStoreService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IMenuService _menuService;
        private readonly IRedisService _redisService;
        private const string PREFIX = "MS_";
        private const string SUB_PREFIX = "SMD_";
        private const string CACHE_KEY_FOR_UPDATE = "Unverified Updated Store";


        public MerchantStoreService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IMenuService menuService,
            IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _menuService = menuService;
            _redisService = redisService;
        }


        /// <summary>
        /// Create Merchant Store
        /// </summary>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        public async Task<MerchantStoreResponse> CreateMerchantStore(MerchantStoreRequest merchantStoreRequest)
        {
            //biz rule

            //Store MerchantStore To Database
            MerchantStore merchantStore = _mapper.Map<MerchantStore>(merchantStoreRequest);
            try
            {
                merchantStore.MerchantStoreId = _utilService.CreateId(PREFIX);
                merchantStore.Status = (int)MerchantStoreStatus.UNVERIFIED_CREATE_MERCHANT_STORE;
                merchantStore.CreatedDate = DateTime.Now;

                _unitOfWork.MerchantStores.Add(merchantStore);

                //create default menu
                _menuService.CreateDefaultMenu(merchantStore.ResidentId, merchantStore.StoreName, merchantStore.MerchantStoreId);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.CreateMerchantStore()]: " + e.Message);

                throw;
            }

            return _mapper.Map<MerchantStoreResponse>(merchantStore);
        }


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MerchantStoreResponse> DeleteMerchantStore(string id)
        {
            //biz rule

            //Check id
            MerchantStore merchantStore;
            try
            {
                merchantStore = await _unitOfWork.MerchantStores.FindAsync(m => m.MerchantStoreId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.DeleteMerchantStore()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore),id);
            }

            //Delete MerchantStore
            try
            {
                merchantStore.Status = (int)MerchantStoreStatus.DELETED_MERCHANT_STORE;

                _unitOfWork.MerchantStores.Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.DeleteMerchantStore()]: " + e.Message);

                throw;
            }

            return _mapper.Map<MerchantStoreResponse>(merchantStore);
        }


        /// <summary>
        /// Get Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendMerchantStoreResponse> GetMerchantStoreById(string id)
        {
            //biz rule

            ExtendMerchantStoreResponse merchantStoreResponse;

            //Get MerchantStore From Database
            try
            {
                MerchantStore merchantStore = await _unitOfWork.MerchantStores.GetMerchantStoreIncludeResidentById(id);
                merchantStoreResponse = _mapper.Map<ExtendMerchantStoreResponse>(merchantStore);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), id);
            }

            return merchantStoreResponse;
        }


        /// <summary>
        /// Request Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExtendMerchantStoreResponse> RequestUpdateMerchantStoreById(string id,
            MerchantStoreUpdateRequest request)
        {
            ExtendMerchantStoreResponse merchantStoreResponse;

            //Check id
            try
            {
                merchantStoreResponse = _mapper.Map<ExtendMerchantStoreResponse>(
                    await _unitOfWork.MerchantStores.FindAsync(m => m.MerchantStoreId.Equals(id)));

                merchantStoreResponse.UpdatedMerchantStore = request;
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.RequestUpdateMerchantStoreById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), id);
            }

            //store to Redis
            _redisService.StoreToList(CACHE_KEY_FOR_UPDATE, merchantStoreResponse,
                new Predicate<MerchantStoreResponse>(ms => ms.MerchantStoreId.Equals(merchantStoreResponse.MerchantStoreId)));

            return merchantStoreResponse;
        }


        /// <summary>
        /// Get Merchant Store By Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<List<MerchantStoreResponse>> GetMerchantStoreByApartmentId(string apartmentId)
        {
            List<MerchantStoreResponse> merchantStoreResponses;

            //Get MerchantStore From Database

            try
            {
                List<MerchantStore> merchantStores = await _unitOfWork.MerchantStores
                                                            .FindListAsync(store => store.ApartmentId.Equals(apartmentId));

                merchantStoreResponses = _mapper.Map<List<MerchantStoreResponse>>(merchantStores);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoreByAppartmentId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), apartmentId);
            }

            return merchantStoreResponses;
        }


        /// <summary>
        /// Add Store Menu Details To Merchant Store
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <param name="storeMenuDetailRequest"></param>
        /// <returns></returns>
        public async Task<List<StoreMenuDetailResponse>> AddStoreMenuDetailsToMerchantStore(string merchantStoreId,
            List<StoreMenuDetailRequest> storeMenuDetailRequest)
        {
            List<StoreMenuDetail> storeMenuDetails = _mapper.Map<List<StoreMenuDetail>>(storeMenuDetailRequest);
            try
            {
                storeMenuDetails.ForEach(storeMenuDetail =>
                {
                    storeMenuDetail.StoreMenuDetailId = _utilService.CreateId(SUB_PREFIX);
                    storeMenuDetail.MerchantStoreId = merchantStoreId;
                    storeMenuDetail.Status = (int)StoreMenuDetailStatus.ACTIVE_STORE_MENU_DETAIL;

                    _unitOfWork.StoreMenuDetails.Add(storeMenuDetail);

                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.AddStoreMenuDetailsToMerchantStore()]: " + e.Message);

                throw;
            }

            return _mapper.Map<List<StoreMenuDetailResponse>>(storeMenuDetails);
        }


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <param name="storeMenuDetailUpdateRequest"></param>
        /// <returns></returns>
        public async Task<StoreMenuDetailResponse> UpdateStoreMenuDetailById(string storeMenuDetailId,
            StoreMenuDetailUpdateRequest storeMenuDetailUpdateRequest)
        {
            StoreMenuDetail storeMenuDetail;
            try
            {
                storeMenuDetail = await _unitOfWork.StoreMenuDetails
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);

                storeMenuDetail.Status = storeMenuDetailUpdateRequest.Status;

                _unitOfWork.StoreMenuDetails.Update(storeMenuDetail);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.UpdateStoreMenuDetailById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);
        }


        /// <summary>
        /// Delete Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <returns></returns>
        public async Task<StoreMenuDetailResponse> DeleteStoreMenuDetailById(string storeMenuDetailId)
        {
            StoreMenuDetail storeMenuDetail;
            try
            {
                storeMenuDetail = await _unitOfWork.StoreMenuDetails
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);

                storeMenuDetail.Status = (int)StoreMenuDetailStatus.DELETED_STORE_MENU_DETAIL;

                _unitOfWork.StoreMenuDetails.Update(storeMenuDetail);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.DeleteStoreMenuDetailById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);
        }


        /// <summary>
        /// Get Merchant Stores By Status
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<MerchantStoreResponse>> GetMerchantStoresByStatus(int status)
        {
            List<MerchantStoreResponse> merchantStoreList = null;

            //get merchantStore from database
            try
            {
                merchantStoreList = _mapper.Map<List<MerchantStoreResponse>>(
                    await _unitOfWork.MerchantStores.FindListAsync(ms => ms.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoresByStatus()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), status);
            }

            return merchantStoreList;
        }


        /// <summary>
        /// Get All Merchant Stores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<ExtendMerchantStoreResponse>> GetAllMerchantStores()
        {
            //Get MerchantStore from database
            List<MerchantStore> merchantStores;
            try
            {
                merchantStores = await _unitOfWork.MerchantStores.GetAllMerchantStoresIncludeResidentAndApartment();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetAllMerchantStores()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), "all");
            }

            return _mapper.Map<List<ExtendMerchantStoreResponse>>(merchantStores);
        }


        /// <summary>
        /// Get Pending Merchant Stores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<ExtendMerchantStoreResponse>> GetPendingMerchantStores()
        {
            List<ExtendMerchantStoreResponse> merchantStoreResponses;

            //get unverified create merchant Store from database
            try
            {
                merchantStoreResponses = _mapper.Map<List<ExtendMerchantStoreResponse>>(
                    await _unitOfWork.MerchantStores.GetUnverifiedMerchantStoreIncludeResident());
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetPendingMerchantStores()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), "pending");
            }

            //get unverified update merchant Store from redis
            List<ExtendMerchantStoreResponse> updateStore = _redisService.GetList<ExtendMerchantStoreResponse>(CACHE_KEY_FOR_UPDATE);

            merchantStoreResponses.AddRange(updateStore);

            return merchantStoreResponses;
        }


        /// <summary>
        /// Get Menus By Store Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendMerchantStoreResponse> GetMenusByStoreId(string id)
        {
            //biz rule

            ExtendMerchantStoreResponse merchantStoreResponse;

            //Get MerchantStore From Database
            try
            {
                MerchantStore merchantStore = await _unitOfWork.MerchantStores.GetMenusByStoreId(id);

                merchantStoreResponse = _mapper.Map<ExtendMerchantStoreResponse>(merchantStore);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMenusByStoreId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), id);
            }

            return merchantStoreResponse;
        }


        /// <summary>
        /// Verify Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCreate"></param>
        /// <param name="isApprove"></param>
        /// <returns></returns>
        public async Task<ExtendMerchantStoreResponse> VerifyMerchantStore(string id, bool isApprove)
        {
            ExtendMerchantStoreResponse merchantStoreResponse = null;
            bool isUpdate = false;

            try
            {
                MerchantStore merchantStore = await _unitOfWork.MerchantStores.FindAsync(ms => ms.MerchantStoreId.Equals(id));

                //get new data for merchant store from redis
                MerchantStoreUpdateRequest ms = _redisService.GetList<ExtendMerchantStoreResponse>(CACHE_KEY_FOR_UPDATE)
                    .Find(ms => ms.MerchantStoreId.Equals(id)).UpdatedMerchantStore;

                if (ms != null)
                {
                    merchantStore = _mapper.Map<MerchantStore>(ms);
                    isUpdate = true;
                }

                merchantStore.Status = isApprove ? (int)MerchantStoreStatus.VERIFIED_MERCHANT_STORE : (int)MerchantStoreStatus.REJECTED_MERCHANT_STORE;

                _unitOfWork.MerchantStores.Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.VerifyMerchantStore()]: " + e.Message);

                throw new EntityNotFoundException(typeof(MerchantStore), id);
            }

            //remove from redis
            if (isUpdate)
            {
                _redisService.DeleteFromList(CACHE_KEY_FOR_UPDATE,
                new Predicate<ExtendMerchantStoreResponse>(ms => ms.MerchantStoreId.Equals(merchantStoreResponse.MerchantStoreId)));
            }

            return merchantStoreResponse;
        }
    }
}
