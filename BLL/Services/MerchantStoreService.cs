﻿using AutoMapper;
using BLL.Dtos;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.MerchantStore;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
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
        public async Task<BaseResponse<MerchantStoreResponse>> CreateMerchantStore(MerchantStoreRequest merchantStoreRequest)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> DeleteMerchantStore(string id)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Get Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendMerchantStoreResponse>> GetMerchantStoreById(string id)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<ExtendMerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Request Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendMerchantStoreResponse>> RequestUpdateMerchantStoreById(string id,
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendMerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //store to Redis
            _redisService.StoreToList(CACHE_KEY_FOR_UPDATE, merchantStoreResponse,
                new Predicate<MerchantStoreResponse>(ms => ms.MerchantStoreId.Equals(merchantStoreResponse.MerchantStoreId)));

            return new BaseResponse<ExtendMerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Get Merchant Store By Store Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> GetMerchantStoreByStoreName(string name)
        {
            //biz rule

            MerchantStoreResponse merchantStoreResponse;

            //Get MerchantStore From Database

            try
            {
                MerchantStore merchantStore = await _unitOfWork.MerchantStores.FindAsync(m => m.StoreName.Equals(name));
                merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoreByStoreName()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Get Merchant Store By Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoreByApartmentId(string apartmentId)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStatus.MERCHANT_NOT_FOUND,
                        ResultMessage = MerchantStatus.MERCHANT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<MerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponses
            };
        }


        /// <summary>
        /// Add Store Menu Details To Merchant Store
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <param name="storeMenuDetailRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<StoreMenuDetailResponse>>> AddStoreMenuDetailsToMerchantStore(string merchantStoreId,
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            List<StoreMenuDetailResponse> storeMenuDetailResponses = _mapper.Map<List<StoreMenuDetailResponse>>(storeMenuDetails);

            return new BaseResponse<List<StoreMenuDetailResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponses
            };
        }


        /// <summary>
        /// Get Store Menu Details By Merchant Store Id
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<StoreMenuDetailResponse>>> GetStoreMenuDetailsByMerchantStoreId(string merchantStoreId)
        {
            List<StoreMenuDetail> storeMenuDetails;
            try
            {
                storeMenuDetails = await _unitOfWork.StoreMenuDetails
                    .FindListAsync(smd => smd.MerchantStoreId == merchantStoreId);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetStoreMenuDetailsByMerchantStoreId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND,
                        ResultMessage = StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Create response
            List<StoreMenuDetailResponse> storeMenuDetailResponses = _mapper.Map<List<StoreMenuDetailResponse>>(storeMenuDetails);

            return new BaseResponse<List<StoreMenuDetailResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponses
            };
        }


        /// <summary>
        /// Get Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<StoreMenuDetailResponse>> GetStoreMenuDetailById(string storeMenuDetailId)
        {
            StoreMenuDetail storeMenuDetail;
            try
            {
                storeMenuDetail = await _unitOfWork.StoreMenuDetails
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetStoreMenuDetailById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND,
                        ResultMessage = StoreMenuDetailStatus.STORE_MENU_DETAIL_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Create response
            StoreMenuDetailResponse storeMenuDetailResponse = _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);

            return new BaseResponse<StoreMenuDetailResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponse
            };
        }


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <param name="storeMenuDetailUpdateRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<StoreMenuDetailResponse>> UpdateStoreMenuDetailById(string storeMenuDetailId,
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            StoreMenuDetailResponse storeMenuDetailResponse = _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);

            return new BaseResponse<StoreMenuDetailResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponse
            };
        }


        /// <summary>
        /// Delete Store Menu Detail By Id
        /// </summary>
        /// <param name="storeMenuDetailId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<StoreMenuDetailResponse>> DeleteStoreMenuDetailById(string storeMenuDetailId)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<StoreMenuDetailResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            StoreMenuDetailResponse storeMenuDetailResponse = _mapper.Map<StoreMenuDetailResponse>(storeMenuDetail);

            return new BaseResponse<StoreMenuDetailResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = storeMenuDetailResponse
            };
        }


        /// <summary>
        /// Get Merchant Stores By Status
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoresByStatus(int status)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<MerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreList
            };
        }


        /// <summary>
        /// Get All Merchant Stores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ExtendMerchantStoreResponse>>> GetAllMerchantStores()
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            List<ExtendMerchantStoreResponse> merchantStoreResponses = _mapper.Map<List<ExtendMerchantStoreResponse>>(merchantStores);

            return new BaseResponse<List<ExtendMerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponses
            };
        }


        /// <summary>
        /// Get Pending Merchant Stores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ExtendMerchantStoreResponse>>> GetPendingMerchantStores()
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendMerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //get unverified update merchant Store from redis
            List<ExtendMerchantStoreResponse> updateStore = _redisService.GetList<ExtendMerchantStoreResponse>(CACHE_KEY_FOR_UPDATE);

            merchantStoreResponses.AddRange(updateStore);

            return new BaseResponse<List<ExtendMerchantStoreResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponses
            };
        }


        /// <summary>
        /// Get Menus By Store Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendMerchantStoreResponse>> GetMenusByStoreId(string id)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendMerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<ExtendMerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Verify Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCreate"></param>
        /// <param name="isApprove"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendMerchantStoreResponse>> VerifyMerchantStore(string id, bool isCreate, bool isApprove)
        {
            ExtendMerchantStoreResponse merchantStoreResponse = null;
            try
            {
                MerchantStore merchantStore = await _unitOfWork.MerchantStores.FindAsync(ms => ms.MerchantStoreId.Equals(id));

                if (!isCreate)
                {
                    //get new data for merchant store from redis
                    MerchantStoreUpdateRequest ms = _redisService.GetList<ExtendMerchantStoreResponse>(CACHE_KEY_FOR_UPDATE).Find(ms => ms.MerchantStoreId.Equals(id)).UpdatedMerchantStore;

                    merchantStore = _mapper.Map<MerchantStore>(ms);
                }

                merchantStore.Status = isApprove ? (int)MerchantStoreStatus.VERIFIED_MERCHANT_STORE : (int)MerchantStoreStatus.REJECTED_MERCHANT_STORE;

                _unitOfWork.MerchantStores.Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.VerifyMerchantStore()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendMerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //remove from redis
            if (!isCreate)
            {
                _redisService.DeleteFromList(CACHE_KEY_FOR_UPDATE,
                new Predicate<ExtendMerchantStoreResponse>(ms => ms.MerchantStoreId.Equals(merchantStoreResponse.MerchantStoreId)));
            }

            return new BaseResponse<ExtendMerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }
    }
}
