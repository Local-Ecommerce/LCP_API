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
        /// <param name="residentId"></param>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        public async Task<MerchantStoreResponse> CreateMerchantStore(string residentId, MerchantStoreRequest merchantStoreRequest)
        {
            //Store MerchantStore To Database
            MerchantStore merchantStore = _mapper.Map<MerchantStore>(merchantStoreRequest);
            try
            {
                merchantStore.MerchantStoreId = _utilService.CreateId(PREFIX);
                merchantStore.Status = (int)MerchantStoreStatus.UNVERIFIED_MERCHANT_STORE;
                merchantStore.CreatedDate = DateTime.Now;
                merchantStore.ResidentId = residentId;

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

                throw new EntityNotFoundException(typeof(MerchantStore), id);
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
        public async Task<object> GetMerchantStore(
            string id, string apartmentId, string residentId,
            string role, int?[] status, int? limit,
            int? page, string sort,
            string[] include)
        {
            PagingModel<MerchantStore> merchantStore;
            string propertyName = default;
            bool isAsc = false;

            residentId = role.Equals(ResidentType.MERCHANT) ? residentId : null;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }
            for (int i = 0; i < include.Length; i++)
            {
                include[i] = !string.IsNullOrEmpty(include[i]) ? _utilService.UpperCaseFirstLetter(include[i]) : null;
            }

            try
            {
                merchantStore = await _unitOfWork.MerchantStores.GetMerchantStore(id, apartmentId, residentId,
                    status, limit, page, isAsc, propertyName, include);

                if (_utilService.IsNullOrEmpty(merchantStore.List))
                    throw new EntityNotFoundException(typeof(Menu), "in the url");
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStore()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendMerchantStoreResponse>
            {
                List = _mapper.Map<List<ExtendMerchantStoreResponse>>(merchantStore.List),
                Page = merchantStore.Page,
                LastPage = merchantStore.LastPage,
                Total = merchantStore.Total,
            };
        }
    }
}
