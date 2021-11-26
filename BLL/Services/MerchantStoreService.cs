using AutoMapper;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.MerchantStore;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;

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
                merchantStore.IsBlock = false;
                merchantStore.IsActive = false;
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
                        ResultCode = (int)MerchantStoreStatus.ERROR,
                        ResultMessage = MerchantStoreStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            //Store MerchantStore To Redis
            _redisService.StoreToList(CACHE_KEY, merchantStoreResponse,
                    new Predicate<MerchantStoreResponse>(a => a.MerchantStoreId == merchantStoreResponse.MerchantStoreId));

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)MerchantStoreStatus.SUCCESS,
                ResultMessage = MerchantStoreStatus.SUCCESS.ToString(),
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
                        ResultCode = (int)MerchantStoreStatus.MERCHANTSTORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANTSTORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete MerchantStore
            try
            {
                merchantStore.IsBlock = true;

                _unitOfWork.Repository<MerchantStore>().Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantStoreService.DeleteMerchantStore()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.ERROR,
                        ResultMessage = MerchantStoreStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            //Store MerchantStore To Redis
            _redisService.StoreToList(CACHE_KEY, merchantStoreResponse,
                    new Predicate<MerchantStoreResponse>(a => a.MerchantStoreId == merchantStoreResponse.MerchantStoreId));

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)MerchantStoreStatus.SUCCESS,
                ResultMessage = MerchantStoreStatus.SUCCESS.ToString(),
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

            MerchantStoreResponse merchantStoreResponse = null;

            //Get MerchantStore From Redis
            merchantStoreResponse = _redisService.GetList<MerchantStoreResponse>(CACHE_KEY)
                .Find(merchant => merchant.MerchantStoreId.Equals(id));

            if (merchantStoreResponse is null)
            {
                //Get MerchantStore From Database
                try
                {
                    MerchantStore merchantStore = await _unitOfWork.Repository<MerchantStore>()
                                                       .FindAsync(m => m.MerchantStoreId.Equals(id));
                    merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);
                }
                catch (Exception e)
                {
                    _logger.Error("[MerchantService.GetMerchantById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<MerchantStoreResponse>
                        {
                            ResultCode = (int)MerchantStoreStatus.MERCHANTSTORE_NOT_FOUND,
                            ResultMessage = MerchantStoreStatus.MERCHANTSTORE_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)MerchantStoreStatus.SUCCESS,
                ResultMessage = MerchantStoreStatus.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }


        /// <summary>
        /// Update Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MerchantStoreResponse>> UpdateMerchantStoreById(string id, MerchantStoreRequest merchantStoreRequest)
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
                        ResultCode = (int)MerchantStoreStatus.MERCHANTSTORE_NOT_FOUND,
                        ResultMessage = MerchantStoreStatus.MERCHANTSTORE_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //update MerchantStore
            try
            {
                merchantStore = _mapper.Map(merchantStoreRequest, merchantStore);

                _unitOfWork.Repository<MerchantStore>().Update(merchantStore);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MerchantService.UpdateMerchantById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MerchantStoreResponse>
                    {
                        ResultCode = (int)MerchantStoreStatus.ERROR,
                        ResultMessage = MerchantStoreStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MerchantStoreResponse merchantStoreResponse = _mapper.Map<MerchantStoreResponse>(merchantStore);

            //Store Merchant To Redis
            _redisService.StoreToList(CACHE_KEY, merchantStoreResponse,
                    new Predicate<MerchantStoreResponse>(a => a.MerchantStoreId == merchantStoreResponse.MerchantStoreId));

            return new BaseResponse<MerchantStoreResponse>
            {
                ResultCode = (int)MerchantStoreStatus.SUCCESS,
                ResultMessage = MerchantStoreStatus.SUCCESS.ToString(),
                Data = merchantStoreResponse
            };
        }
    }
}
