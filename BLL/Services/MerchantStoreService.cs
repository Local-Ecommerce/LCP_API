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
    }
}
