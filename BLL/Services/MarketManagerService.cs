using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.MarketManager;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MarketManagerService : IMarketManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "MarketManager";

        public MarketManagerService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService,
            IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
        }

        /// <summary>
        /// Create MarketManager
        /// </summary>
        /// <param name="marketManagerRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MarketManagerResponse>> CreateMarketManager(MarketManagerRequest marketManagerRequest)
        {

            //biz rule

            //Store MarketManager To Dabatabase
            MarketManager marketManager = _mapper.Map<MarketManager>(marketManagerRequest);

            try
            {
                marketManager.MarketManagerId = _utilService.Create16Alphanumeric();
                marketManager.Status = (int)MarketManagerStatus.ACTIVE_MARKETMANAGER;

                _unitOfWork.Repository<MarketManager>().Add(marketManager);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MarketManagerService.CreateMarketManager()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MarketManagerResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MarketManagerResponse marketManagerResponse = _mapper.Map<MarketManagerResponse>(marketManager);

            //Store MarketManager To Redis
            _redisService.StoreToList(CACHE_KEY, marketManagerResponse,
                    new Predicate<MarketManagerResponse>(a => a.MarketManagerId == marketManagerResponse.MarketManagerId));

            return new BaseResponse<MarketManagerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponse
            };

        }

        /// <summary>
        /// Delete MarketManager
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MarketManagerResponse>> DeleteMarketManager(string id)
        {
            //biz rule

            //Check id
            MarketManager marketManager;
            try
            {
                marketManager = await _unitOfWork.Repository<MarketManager>()
                                       .FindAsync(local => local.MarketManagerId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MarketManagerService.DeleteMarketManager()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MarketManager>
                    {
                        ResultCode = (int)MarketManagerStatus.MARKETMANAGER_NOT_FOUND,
                        ResultMessage = MarketManagerStatus.MARKETMANAGER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete MarketManager
            try
            {
                marketManager.Status = (int)MarketManagerStatus.DELETED_MARKETMANAGER;

                _unitOfWork.Repository<MarketManager>().Update(marketManager);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MarketManagerService.DeleteMarketManager()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MarketManager>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            MarketManagerResponse marketManagerResponse = _mapper.Map<MarketManagerResponse>(marketManager);

            //Store MarketManager To Redis
            _redisService.StoreToList(CACHE_KEY, marketManagerResponse,
                    new Predicate<MarketManagerResponse>(a => a.MarketManagerId == marketManagerResponse.MarketManagerId));

            return new BaseResponse<MarketManagerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponse
            };

        }

        /// <summary>
        /// Get MarketManager By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<MarketManagerResponse>>> GetMarketManagerByAccountId(string accountId)
        {
            List<MarketManagerResponse> marketManagerResponses = null;

            //Get MarketManager From Redis
            marketManagerResponses = _redisService.GetList<MarketManagerResponse>(CACHE_KEY)
                                            .Where(MarketManager => MarketManager.AccountId.Equals(accountId))
                                            .ToList();

            //Get MarketManager From Database
            if (marketManagerResponses is null)
            {
                try
                {
                    List<MarketManager> marketManagers = await _unitOfWork.Repository<MarketManager>().
                                                            FindListAsync(MarketManager => MarketManager.AccountId.Equals(accountId));

                    marketManagerResponses = _mapper.Map<List<MarketManagerResponse>>(marketManagers);
                }
                catch (Exception e)
                {
                    _logger.Error("[MarketManagerService.GetMarketManagerByAccountId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<MarketManagerResponse>
                        {
                            ResultCode = (int)MarketManagerStatus.MARKETMANAGER_NOT_FOUND,
                            ResultMessage = MarketManagerStatus.MARKETMANAGER_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<List<MarketManagerResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponses
            };
        }

        /// <summary>
        /// Get Market Manager By Appartment Id
        /// </summary>
        /// <param name="appartmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<MarketManagerResponse>>> GetMarketManagerByAppartmentId(string appartmentId)
        {
            List<MarketManagerResponse> marketManagerResponses = null;

            //Get MarketManager From Redis
            marketManagerResponses = _redisService.GetList<MarketManagerResponse>(CACHE_KEY)
                                            .Where(MarketManager => MarketManager.AparmentId.Equals(appartmentId))
                                            .ToList();

            //Get MarketManager From Database
            if (marketManagerResponses is null)
            {
                try
                {
                    List<MarketManager> marketManagers = await _unitOfWork.Repository<MarketManager>().
                                                            FindListAsync(marketManager => marketManager.AparmentId.Equals(appartmentId));

                    marketManagerResponses = _mapper.Map<List<MarketManagerResponse>>(marketManagers);
                }
                catch (Exception e)
                {
                    _logger.Error("[MarketManagerService.GetMarketManagerByAppartmentId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<MarketManagerResponse>
                        {
                            ResultCode = (int)MarketManagerStatus.MARKETMANAGER_NOT_FOUND,
                            ResultMessage = MarketManagerStatus.MARKETMANAGER_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<List<MarketManagerResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponses
            };
        }

        /// <summary>
        /// Get MarketManager By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MarketManagerResponse>> GetMarketManagerById(string id)
        {
            //biz rule


            MarketManagerResponse marketManagerResponse = null;
            //Get MarketManager From Redis
            marketManagerResponse = _redisService.GetList<MarketManagerResponse>(CACHE_KEY)
                                            .Find(local => local.MarketManagerId.Equals(id));

            //Get MarketManager From Database
            if (marketManagerResponse is null)
            {
                try
                {
                    MarketManager marketManager = await _unitOfWork.Repository<MarketManager>().
                                                            FindAsync(local => local.MarketManagerId.Equals(id));

                    marketManagerResponse = _mapper.Map<MarketManagerResponse>(marketManager);
                }
                catch (Exception e)
                {
                    _logger.Error("[MarketManagerService.GetMarketManagerById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<MarketManagerResponse>
                        {
                            ResultCode = (int)MarketManagerStatus.MARKETMANAGER_NOT_FOUND,
                            ResultMessage = MarketManagerStatus.MARKETMANAGER_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<MarketManagerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponse
            };
        }

        /// <summary>
        /// Update MarketManager
        /// </summary>
        /// <param name="id"></param>
        /// <param name="marketManagerRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MarketManagerResponse>> UpdateMarketManagerById(string id, MarketManagerRequest marketManagerRequest)
        {
            //biz ruie

            //Check id
            MarketManager marketManager;
            try
            {
                marketManager = await _unitOfWork.Repository<MarketManager>()
                                       .FindAsync(local => local.MarketManagerId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MarketManagerService.UpdateMarketManager()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<MarketManagerResponse>
                {
                    ResultCode = (int)MarketManagerStatus.MARKETMANAGER_NOT_FOUND,
                    ResultMessage = MarketManagerStatus.MARKETMANAGER_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update MarketManager To DB
            try
            {
                marketManager = _mapper.Map(marketManagerRequest, marketManager);

                _unitOfWork.Repository<MarketManager>().Update(marketManager);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MarketManagerService.UpdateMarketManager()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<MarketManagerResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            MarketManagerResponse marketManagerResponse = _mapper.Map<MarketManagerResponse>(marketManager);

            //Store Reponse To Redis
            _redisService.StoreToList(CACHE_KEY, marketManagerResponse,
                    new Predicate<MarketManagerResponse>(a => a.MarketManagerId == marketManagerResponse.MarketManagerId));

            return new BaseResponse<MarketManagerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponse
            };
        }
    }
}
