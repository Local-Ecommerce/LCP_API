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
using System.Linq;

namespace BLL.Services
{
    public class MerchantStoreService : IMerchantStoreService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IMenuService _menuService;
        private readonly IFirebaseService _firebaseService;
        private readonly IRedisService _redisService;
        private const string PREFIX = "MS_";
        private const string TYPE = "Merchant Store";
        private const string CACHE_KEY_FOR_UPDATE = "Unverified Updated Store";


        public MerchantStoreService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IMenuService menuService,
            IFirebaseService firebaseService,
            IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _menuService = menuService;
            _firebaseService = firebaseService;
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
                //check resident Id
                Resident resident = await _unitOfWork.Residents.FindAsync(r => r.ResidentId.Equals(residentId));
                if (!resident.Type.Equals(ResidentType.MERCHANT))
                    throw new BusinessException($"Resident {residentId} is not a merchant.");

                merchantStore.MerchantStoreId = _utilService.CreateId(PREFIX);
                merchantStore.Status = (int)MerchantStoreStatus.UNVERIFIED_MERCHANT_STORE;
                merchantStore.CreatedDate = DateTime.Now;
                merchantStore.ResidentId = residentId;
                merchantStore.ApartmentId = resident.ApartmentId;
                merchantStore.StoreImage = _firebaseService
                                .UploadFileToFirebase(merchantStoreRequest.StoreName, TYPE, merchantStore.MerchantStoreId, "Image")
                                .Result;

                _unitOfWork.MerchantStores.Add(merchantStore);

                //create default menu
                _menuService.CreateDefaultMenu(merchantStore.StoreName, merchantStore.MerchantStoreId);

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
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task DeleteMerchantStore(string id, string residentId)
        {
            //Check id
            MerchantStore merchantStore;
            try
            {
                merchantStore = await _unitOfWork.MerchantStores.FindAsync(m => m.MerchantStoreId.Equals(id)
                                                                        && m.ResidentId.Equals(residentId));
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
        }


        /// <summary>
        /// Update Merchant Store By Id
        /// /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task UpdateMerchantStoreById(string id,
            MerchantStoreRequest request, string residentId)
        {
            MerchantStore store;
            MerchantStoreResponse storeResponse;
            //Check id
            try
            {
                store = await _unitOfWork.MerchantStores.FindAsync(m => m.MerchantStoreId.Equals(id)
                                                                    && m.ResidentId.Equals(residentId));

                //add info store to redis
                storeResponse = _mapper.Map<MerchantStoreResponse>(store);
                storeResponse.StoreName = !string.IsNullOrEmpty(request.StoreName) ? request.StoreName : storeResponse.StoreName;
                storeResponse.StoreImage = _firebaseService
                                .UploadFileToFirebase(request.StoreImage, TYPE, id, "Image")
                                .Result;
                storeResponse.UpdatedDate = DateTime.Now;
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.UpdateMerchantStoreById()]: " + e.Message);
                throw;
            }

            //store to Redis
            _redisService.StoreToList(CACHE_KEY_FOR_UPDATE, storeResponse,
                new Predicate<MerchantStoreResponse>(ms => ms.MerchantStoreId.Equals(id)));
        }


        /// <summary>
        /// Verify Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCreate"></param>
        /// <param name="isApprove"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<ExtendMerchantStoreResponse> VerifyMerchantStore(string id, bool isApprove, string residentId)
        {
            ExtendMerchantStoreResponse merchantStoreResponse = null;
            bool isUpdate = false;

            try
            {
                //check market manager's permission
                Resident resident = await _unitOfWork.Residents.FindAsync(r => r.ResidentId.Equals(residentId));

                MerchantStore merchantStore = await _unitOfWork.MerchantStores.FindAsync(ms => ms.MerchantStoreId.Equals(id)
                                                                                    && ms.ApartmentId.Equals(resident.ApartmentId));

                if (merchantStore is null)
                    throw new BusinessException($"Market Manager {residentId} does not have right to verify store {id}");

                //get new data for merchant store from redis
                MerchantStoreResponse ms = _redisService.GetList<MerchantStoreResponse>(CACHE_KEY_FOR_UPDATE)
                    .Find(ms => ms.MerchantStoreId.Equals(id));

                if (isApprove)
                {
                    if (ms != null)
                    {
                        //map new data
                        merchantStore.StoreName = !string.IsNullOrEmpty(ms.StoreName) ? ms.StoreName : merchantStore.StoreName;
                        merchantStore.StoreImage = !string.IsNullOrEmpty(ms.StoreImage) ? ms.StoreImage : merchantStore.StoreImage;

                        isUpdate = true;
                    }
                    else merchantStore.Status = (int)MerchantStoreStatus.VERIFIED_MERCHANT_STORE;

                }
                else
                    merchantStore.Status = (int)MerchantStoreStatus.REJECTED_MERCHANT_STORE;

                _unitOfWork.MerchantStores.Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();

                merchantStoreResponse = _mapper.Map(merchantStore, merchantStoreResponse);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.VerifyMerchantStore()]: " + e.Message);
                throw;
            }

            //remove from redis
            if (isUpdate)
            {
                _redisService.DeleteFromList(CACHE_KEY_FOR_UPDATE,
                new Predicate<MerchantStoreResponse>(ms => ms.MerchantStoreId.Equals(merchantStoreResponse.MerchantStoreId)));
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
        public async Task<object> GetMerchantStores(
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
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStore()]" + e.Message);
                throw;
            }

            List<ExtendMerchantStoreResponse> responses = _mapper.Map<List<ExtendMerchantStoreResponse>>(merchantStore.List);

            // get new stores's info if update
            if (status.Contains((int)MerchantStoreStatus.UNVERIFIED_MERCHANT_STORE))
            {
                foreach (var response in responses)
                {
                    response.UpdatedMerchantStore = _redisService
                            .GetList<MerchantStoreResponse>(CACHE_KEY_FOR_UPDATE)
                            .Find(store => store.MerchantStoreId == response.MerchantStoreId);
                }
            }

            return new PagingModel<ExtendMerchantStoreResponse>
            {
                List = responses,
                Page = merchantStore.Page,
                LastPage = merchantStore.LastPage,
                Total = merchantStore.Total,
            };
        }


        /// <summary>
        /// Get Unverified Merchant Stores
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<List<ExtendMerchantStoreResponse>> GetUnverifiedMerchantStores(string residentId)
        {
            List<ExtendMerchantStoreResponse> responses;
            try
            {
                Resident resident = await _unitOfWork.Residents.FindAsync(r => r.ResidentId.Equals(residentId));

                //get updated store from redis
                List<MerchantStoreResponse> storeRedis = _redisService.GetList<MerchantStoreResponse>(CACHE_KEY_FOR_UPDATE);

                List<string> ids = storeRedis.Select(ms => ms.MerchantStoreId).ToList();

                //get store from database
                List<MerchantStore> stores = await _unitOfWork.MerchantStores.GetMerchantStoresByIdsAndApartmentId(ids, resident.ApartmentId);

                //mapping data
                responses = _mapper.Map<List<ExtendMerchantStoreResponse>>(stores);
                foreach (var response in responses)
                {
                    response.UpdatedMerchantStore = storeRedis.Where(ms => ms.MerchantStoreId.Equals(response.MerchantStoreId)).First();
                }
            }
            catch (Exception e)
            {
                _logger.Error("MerchantStoreService.GetUnverifiedMerchantStores(): " + e.Message);
                throw;
            }

            return responses;
        }
    }
}
