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
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MarketManagerService : IMarketManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "MM_";

        public MarketManagerService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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
                marketManager.MarketManagerId = _utilService.CreateId(PREFIX);
                marketManager.Status = (int)MarketManagerStatus.ACTIVE_MARKETMANAGER;

                _unitOfWork.MarketManagers.Add(marketManager);

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
                marketManager = await _unitOfWork.MarketManagers
                                       .FindAsync(mar => mar.MarketManagerId.Equals(id));
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

                _unitOfWork.MarketManagers.Update(marketManager);

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
        public async Task<BaseResponse<MarketManagerResponse>> GetMarketManagerByAccountId(string accountId)
        {
            MarketManagerResponse marketManagerResponse;

            //Get MarketManager From Database

            try
            {
                MarketManager marketManagers = await _unitOfWork.MarketManagers.
                                                        FindAsync(MarketManager => MarketManager.AccountId.Equals(accountId));

                marketManagerResponse = _mapper.Map<MarketManagerResponse>(marketManagers);
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

            return new BaseResponse<MarketManagerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponse
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


            MarketManagerResponse marketManagerResponse;

            //Get MarketManager From Database

            try
            {
                MarketManager marketManager = await _unitOfWork.MarketManagers.
                                                        FindAsync(mar => mar.MarketManagerId.Equals(id));

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
                marketManager = await _unitOfWork.MarketManagers
                                       .FindAsync(mar => mar.MarketManagerId.Equals(id));
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

                _unitOfWork.MarketManagers.Update(marketManager);

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

            return new BaseResponse<MarketManagerResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerResponse
            };
        }


        /// <summary>
        /// Get Market Manager By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<MarketManagerResponse>>> GetMarketManagerByStatus(int status)
        {
            List<MarketManagerResponse> marketManagerList = null;

            //get marketManager from database
            try
            {
                marketManagerList = _mapper.Map<List<MarketManagerResponse>>(
                    await _unitOfWork.MarketManagers
                                     .FindListAsync(mar => mar.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[MarketManagerService.GetMarketManagersByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<MarketManagerResponse>
                    {
                        ResultCode = (int)MarketManagerStatus.MARKETMANAGER_NOT_FOUND,
                        ResultMessage = MarketManagerStatus.MARKETMANAGER_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<MarketManagerResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = marketManagerList
            };
        }

    }
}
