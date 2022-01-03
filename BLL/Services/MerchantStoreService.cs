using AutoMapper;
using BLL.Dtos;
using BLL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.MerchantStore;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BLL.Dtos.StoreMenuDetail;

namespace BLL.Services
{
    public class MerchantStoreService : IMerchantStoreService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "MerchantStore";

        public MerchantStoreService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
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
                merchantStore.MerchantStoreId = _utilService.Create16Alphanumeric();
                merchantStore.Status = (int)MerchantStoreStatus.UNVERIFIED_CREATE_MERCHANT_STORE;
                merchantStore.CreatedDate = DateTime.Now;

                _unitOfWork.Repository<MerchantStore>().Add(merchantStore);

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
                merchantStore = await _unitOfWork.Repository<MerchantStore>().
                                                      FindAsync(m => m.MerchantStoreId.Equals(id));
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

                _unitOfWork.Repository<MerchantStore>().Update(merchantStore);

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
        public async Task<BaseResponse<MerchantStoreResponse>> GetMerchantStoreById(string id)
        {
            //biz rule

            MerchantStoreResponse merchantStoreResponse;

            //Get MerchantStore From Database
            try
            {
                MerchantStore merchantStore = await _unitOfWork.Repository<MerchantStore>()
                                                   .FindAsync(m => m.MerchantStoreId.Equals(id));
                merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);
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

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> UpdateMerchantStoreById(string id,
            MerchantStoreRequest merchantStoreRequest)
        {
            MerchantStore merchantStore;

            //Check id
            try
            {
                merchantStore = await _unitOfWork.Repository<MerchantStore>().
                                             FindAsync(m => m.MerchantStoreId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.UpdateMerchantStoreById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANT_STORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //update MerchantStore
            try
            {
                merchantStore = _mapper.Map(merchantStoreRequest, merchantStore);
                merchantStore.Status = (int)MerchantStoreStatus.UNVERIFIED_UPDATE_MERCHANT_STORE;

                _unitOfWork.Repository<MerchantStore>().Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.UpdateMerchantById()]: " + e.Message);

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
                MerchantStore merchantStore = await _unitOfWork.Repository<MerchantStore>()
                                                   .FindAsync(m => m.StoreName.Equals(name));
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
        /// Get Merchant Store By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetMerchantStoreByMerchantId(string merchantId)
        {
            List<MerchantStoreResponse> merchantStoreResponses;

            //Get MerchantStore From Database

            try
            {
                List<MerchantStore> merchantStores = await _unitOfWork.Repository<MerchantStore>().
                                                        FindListAsync
                                                        (store => store.MerchantId.Equals(merchantId));

                merchantStoreResponses = _mapper.Map<List<MerchantStoreResponse>>(merchantStores);
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetMerchantStoreByMerchantId()]: " + e.Message);

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
                List<MerchantStore> merchantStores = await _unitOfWork.Repository<MerchantStore>().
                                                        FindListAsync
                                                        (store => store.AparmentId.Equals(apartmentId));

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
                    storeMenuDetail.StoreMenuDetailId = _utilService.Create16Alphanumeric();
                    storeMenuDetail.CreatedDate = DateTime.Now;
                    storeMenuDetail.MerchantStoreId = merchantStoreId;
                    storeMenuDetail.Status = (int)StoreMenuDetailStatus.ACTIVE_STORE_MENU_DETAIL;

                    _unitOfWork.Repository<StoreMenuDetail>().Add(storeMenuDetail);

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
                storeMenuDetails = await _unitOfWork.Repository<StoreMenuDetail>()
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
                storeMenuDetail = await _unitOfWork.Repository<StoreMenuDetail>()
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
                storeMenuDetail = await _unitOfWork.Repository<StoreMenuDetail>()
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);

                storeMenuDetail.Status = storeMenuDetailUpdateRequest.Status;
                storeMenuDetail.TimeStart = storeMenuDetailUpdateRequest.TimeStart;
                storeMenuDetail.TimeEnd = storeMenuDetailUpdateRequest.TimeEnd;

                _unitOfWork.Repository<StoreMenuDetail>().Update(storeMenuDetail);
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
                storeMenuDetail = await _unitOfWork.Repository<StoreMenuDetail>()
                    .FindAsync(smd => smd.StoreMenuDetailId == storeMenuDetailId);

                storeMenuDetail.Status = (int)StoreMenuDetailStatus.DELETED_STORE_MENU_DETAIL;

                _unitOfWork.Repository<StoreMenuDetail>().Update(storeMenuDetail);
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
        /// Get Verified Merchant Stores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<MerchantStoreResponse>>> GetVerifiedMerchantStores()
        {
            List<MerchantStoreResponse> merchantStoreList = null;

            //get systemCategory from database
            try
            {
                merchantStoreList = _mapper.Map<List<MerchantStoreResponse>>(
                    await _unitOfWork.Repository<MerchantStore>()
                                     .FindListAsync(ms => ms.MerchantStoreId != null 
                                     && ms.Status == (int)MerchantStoreStatus.VERIFIED_MERCHANT_STORE));
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.GetVerifiedMerchantStores()]: " + e.Message);

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
    }
}
