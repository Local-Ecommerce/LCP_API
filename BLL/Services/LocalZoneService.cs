using AutoMapper;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.LocalZone;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LocalZoneService : ILocalZoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "LocalZone";

        public LocalZoneService(IUnitOfWork unitOfWork,
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
        /// Create LocalZone
        /// </summary>
        /// <param name="localZoneRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<LocalZoneResponse>> CreateLocalZone(LocalZoneRequest localZoneRequest)
        {

            //biz rule

            //Store LocalZone To Dabatabase
            LocalZone localZone = _mapper.Map<LocalZone>(localZoneRequest);

            try
            {
                localZone.LocalZoneId = _utilService.Create16Alphanumeric();
                localZone.IsActive = true;

                _unitOfWork.Repository<LocalZone>().Add(localZone);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[LocalZoneService.CreateLocalZone()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<LocalZoneResponse>
                    {
                        ResultCode = (int)LocalZoneStatus.ERROR,
                        ResultMessage = LocalZoneStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            LocalZoneResponse localZoneResponse = _mapper.Map<LocalZoneResponse>(localZone);

            //Store LocalZone To Redis
            StoreLocalZoneToRedis(localZoneResponse);

            return new BaseResponse<LocalZoneResponse>
            {
                ResultCode = (int)LocalZoneStatus.SUCCESS,
                ResultMessage = LocalZoneStatus.SUCCESS.ToString(),
                Data = localZoneResponse
            };

        }


        /// <summary>
        /// Delete LocalZone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<LocalZoneResponse>> DeleteLocalZone(string id)
        {
            //biz rule

            //Check id
            LocalZone localZone;
            try
            {
                localZone = await _unitOfWork.Repository<LocalZone>()
                                       .FindAsync(local => local.LocalZoneId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[LocalZoneService.DeleteLocalZone()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<LocalZone>
                    {
                        ResultCode = (int)LocalZoneStatus.LOCALZONE_NOT_FONUND,
                        ResultMessage = LocalZoneStatus.LOCALZONE_NOT_FONUND.ToString(),
                        Data = default
                    });
            }

            //Delete LocalZone
            try
            {
                localZone.IsActive = false;

                _unitOfWork.Repository<LocalZone>().Update(localZone);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[LocalZoneService.DeleteLocalZone()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<LocalZone>
                    {
                        ResultCode = (int)LocalZoneStatus.ERROR,
                        ResultMessage = LocalZoneStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            LocalZoneResponse localZoneResponse = _mapper.Map<LocalZoneResponse>(localZone);

            //Store LocalZone To Redis
            StoreLocalZoneToRedis(localZoneResponse);

            return new BaseResponse<LocalZoneResponse>
            {
                ResultCode = (int)LocalZoneStatus.SUCCESS,
                ResultMessage = LocalZoneStatus.SUCCESS.ToString(),
                Data = localZoneResponse
            };

        }


        /// <summary>
        /// Get LocalZone By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<LocalZoneResponse>> GetLocalZoneById(string id)
        {
            //biz rule


            LocalZoneResponse localZoneResponse = null;
            //Get LocalZone From Redis
            localZoneResponse = _redisService.GetList<LocalZoneResponse>(CACHE_KEY)
                                            .Find(local => local.LocalZoneId.Equals(id));

            //Get LocalZone From Database
            if(localZoneResponse is null)
            {
                try
                {
                    LocalZone localZone = await _unitOfWork.Repository<LocalZone>().
                                                            FindAsync(local => local.LocalZoneId.Equals(id));

                    localZoneResponse = _mapper.Map<LocalZoneResponse>(localZone);
                }
                catch (Exception e)
                {
                    _logger.Error("[LocalZoneService.GetLocalZoneById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<LocalZoneResponse>
                        {
                            ResultCode = (int)LocalZoneStatus.LOCALZONE_NOT_FONUND,
                            ResultMessage = LocalZoneStatus.LOCALZONE_NOT_FONUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<LocalZoneResponse>
            {
                ResultCode = (int)LocalZoneStatus.SUCCESS,
                ResultMessage = LocalZoneStatus.SUCCESS.ToString(),
                Data = localZoneResponse
            };
        }


        /// <summary>
        /// Update LocalZone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="localZoneRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<LocalZoneResponse>> UpdateLocalZoneById(string id, LocalZoneRequest localZoneRequest)
        {
            //biz ruie

            //Check id
            LocalZone localZone;
            try
            {
                localZone = await _unitOfWork.Repository<LocalZone>()
                                       .FindAsync(local => local.LocalZoneId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[LocalZoneService.UpdateLocalZone()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<LocalZoneResponse>
                {
                    ResultCode = (int)LocalZoneStatus.LOCALZONE_NOT_FONUND,
                    ResultMessage = LocalZoneStatus.LOCALZONE_NOT_FONUND.ToString(),
                    Data = default
                });
            }

            //Update LocalZone To DB
            try
            {
                localZone = _mapper.Map(localZoneRequest, localZone);

                _unitOfWork.Repository<LocalZone>().Update(localZone);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[LocalZoneService.UpdateLocalZone()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<LocalZoneResponse>
                {
                    ResultCode = (int)LocalZoneStatus.ERROR,
                    ResultMessage = LocalZoneStatus.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            LocalZoneResponse localZoneResponse = _mapper.Map<LocalZoneResponse>(localZone);

            //Store Reponse To Redis
            StoreLocalZoneToRedis(localZoneResponse);

            return new BaseResponse<LocalZoneResponse>
            {
                ResultCode = (int)LocalZoneStatus.SUCCESS,
                ResultMessage = LocalZoneStatus.SUCCESS.ToString(),
                Data = localZoneResponse
            };
        }


        /// <summary>
        /// Store LocalZone To Redis
        /// </summary>
        /// <param name="localZone"></param>
        public void StoreLocalZoneToRedis(LocalZoneResponse localZone)
        {

            List<LocalZoneResponse> localZones = _redisService.GetList<LocalZoneResponse>(CACHE_KEY);

            //Check if localZones null or not
            if (_utilService.IsNullOrEmpty(localZones))
            {
                localZones = new List<LocalZoneResponse>();
            }

            LocalZoneResponse local = localZones.Find(loc => loc.LocalZoneId.Equals(localZone.LocalZoneId));

            //Check localZone exist or not
            if (local != null)
            {
                localZones.Remove(local);
            }

            localZones.Add(localZone);

            _redisService.StoreList(CACHE_KEY, localZones);
        }
    }
}
