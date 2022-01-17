using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.POI;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PoiService : IPoiService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "POI_";

        public PoiService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }

        /// <summary>
        /// Create Poi
        /// </summary>
        /// <param name="poiRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PoiResponse>> CreatePoi(PoiRequest poiRequest)
        {
            Poi poi = _mapper.Map<Poi>(poiRequest);

            try
            {
                poi.PoiId = _utilService.CreateId(PREFIX);
                poi.ReleaseDate = DateTime.Now;
                poi.Status = (int)PoiStatus.ACTIVE_POI;

                _unitOfWork.Pois.Add(poi);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.CreatePoi()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<PoiResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }
            //Create Response
            PoiResponse poiResponse = _mapper.Map<PoiResponse>(poi);

            //Store Poi to Redis

            return new BaseResponse<PoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiResponse
            };
        }


        /// <summary>
        /// Get Poi by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PoiResponse>> GetPoiById(string id)
        {
            PoiResponse poiResponse = null;
            //Get poi from Redis

            //Get poi from DB
            if (poiResponse is null)
            {
                try
                {
                    Poi poi = await _unitOfWork.Pois.GetPoiIncludeResidentByPoiId(id);

                    poiResponse = _mapper.Map<PoiResponse>(poi);
                }
                catch (Exception e)
                {
                    _logger.Error("[PoiService.GetPoiById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Poi>
                    {
                        ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                        ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                        Data = default
                    });
                }
            }

            return new BaseResponse<PoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiResponse
            };
        }


        /// <summary>
        /// Get POI By Release Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<PoiResponse>>> GetPoiByReleaseDate(DateTime date)
        {
            List<PoiResponse> poiResponses = null;


            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(poiResponses))
            {
                try
                {
                    List<Poi> poi = await _unitOfWork.Pois.FindListAsync(poi => poi.ReleaseDate.Value.Date == date.Date);

                    poiResponses = _mapper.Map<List<PoiResponse>>(poi);
                }
                catch (Exception e)
                {
                    _logger.Error("[PoiService.GetPoiByReleasedDate()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Poi>
                    {
                        ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                        ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                        Data = default
                    });
                }
            }

            return new BaseResponse<List<PoiResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiResponses
            };
        }


        /// <summary>
        /// Get Poi by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<PoiResponse>>> GetPoiByApartmentId(string apartmentId)
        {
            List<PoiResponse> poiResponses = null;

            //Get Poi from Redis

            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(poiResponses))
            {
                try
                {
                    List<Poi> poi = await _unitOfWork.Pois.FindListAsync(poi => poi.ApartmentId.Equals(apartmentId));

                    poiResponses = _mapper.Map<List<PoiResponse>>(poi);
                }
                catch (Exception e)
                {
                    _logger.Error("[PoiService.GetPoiByApartmentId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Poi>
                    {
                        ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                        ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                        Data = default
                    });
                }
            }

            return new BaseResponse<List<PoiResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiResponses
            };
        }


        /// <summary>
        /// Update Poi by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="poiRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PoiResponse>> UpdatePoiById(string id, PoiRequest poiRequest)
        {
            Poi poi;
            //Find Poi
            try
            {
                poi = await _unitOfWork.Pois.FindAsync(poi => poi.PoiId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.UpdatePoiById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Poi>
                {
                    ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                    ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update Poi to DB
            try
            {
                poi = _mapper.Map(poiRequest, poi);
                poi.Status = (int)PoiStatus.ACTIVE_POI;

                _unitOfWork.Pois.Update(poi);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.UpdatePoiById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Poi>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create response
            PoiResponse poiResponse = _mapper.Map<PoiResponse>(poi);

            //Store to Redis

            return new BaseResponse<PoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiResponse
            };
        }


        /// <summary>
        /// Delete POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<PoiResponse>> DeletePoiById(string id)
        {
            //Check id
            Poi poi;
            try
            {
                poi = await _unitOfWork.Pois.FindAsync(poi => poi.PoiId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.DeletePoiById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Poi>
                {
                    ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                    ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Delete Poi
            try
            {
                poi.Status = (int)PoiStatus.INACTIVE_POI;

                _unitOfWork.Pois.Update(poi);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.DeletePoiById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Poi>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create response
            PoiResponse poiResponse = _mapper.Map<PoiResponse>(poi);

            //Store Poi to redis

            return new BaseResponse<PoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiResponse
            };
        }


        /// <summary>
        /// Get Pois By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<PoiResponse>>> GetPoisByStatus(int status)
        {
            List<PoiResponse> poiList = null;

            //get Poi from database
            try
            {
                poiList = _mapper.Map<List<PoiResponse>>(
                    await _unitOfWork.Pois.FindListAsync(Poi => Poi.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.GetPoisByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<PoiResponse>
                    {
                        ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                        ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<PoiResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiList
            };
        }

        /// <summary>
        /// Get All Poi
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<PoiResponse>>> GetAllPoi()
        {
            List<Poi> pois;

            try
            {
                pois = await _unitOfWork.Pois.GetAllPoisIncludeApartmentAndResident();
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.GetAllPoi()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<PoiResponse>
                    {
                        ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                        ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            List<PoiResponse> poiResponses = _mapper.Map<List<PoiResponse>>(pois);

            return new BaseResponse<List<PoiResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = poiResponses
            };
        }
    }
}
