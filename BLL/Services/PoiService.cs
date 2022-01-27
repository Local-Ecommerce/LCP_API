using AutoMapper;
using DAL.Constants;
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
        public async Task<BaseResponse<ExtendPoiResponse>> CreatePoi(PoiRequest poiRequest)
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

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ExtendPoiResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }
            //Create Response
            ExtendPoiResponse ExtendPoiResponse = _mapper.Map<ExtendPoiResponse>(poi);

            //Store Poi to Redis

            return new BaseResponse<ExtendPoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendPoiResponse
            };
        }


        /// <summary>
        /// Get Poi by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendPoiResponse>> GetPoiById(string id)
        {
            ExtendPoiResponse ExtendPoiResponse = null;
            //Get poi from Redis

            //Get poi from DB
            if (ExtendPoiResponse is null)
            {
                try
                {
                    Poi poi = await _unitOfWork.Pois.GetPoiIncludeResidentAndApartMentByPoiId(id);

                    ExtendPoiResponse = _mapper.Map<ExtendPoiResponse>(poi);
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

            return new BaseResponse<ExtendPoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendPoiResponse
            };
        }


        /// <summary>
        /// Get POI By Release Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ExtendPoiResponse>>> GetPoiByReleaseDate(DateTime date)
        {
            List<ExtendPoiResponse> ExtendPoiResponses = null;


            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(ExtendPoiResponses))
            {
                try
                {
                    List<Poi> poi = await _unitOfWork.Pois.FindListAsync(poi => poi.ReleaseDate.Value.Date == date.Date);

                    ExtendPoiResponses = _mapper.Map<List<ExtendPoiResponse>>(poi);
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

            return new BaseResponse<List<ExtendPoiResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendPoiResponses
            };
        }


        /// <summary>
        /// Get Poi by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ExtendPoiResponse>>> GetPoiByApartmentId(string apartmentId)
        {
            List<ExtendPoiResponse> ExtendPoiResponses = null;

            //Get Poi from Redis

            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(ExtendPoiResponses))
            {
                try
                {
                    List<Poi> poi = await _unitOfWork.Pois.FindListAsync(poi => poi.ApartmentId.Equals(apartmentId));

                    ExtendPoiResponses = _mapper.Map<List<ExtendPoiResponse>>(poi);
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

            return new BaseResponse<List<ExtendPoiResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendPoiResponses
            };
        }


        /// <summary>
        /// Update Poi by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poiUpdateRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendPoiResponse>> UpdatePoiById(string id, PoiUpdateRequest poiUpdateRequest)
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
                poi = _mapper.Map(poiUpdateRequest, poi);

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
            ExtendPoiResponse ExtendPoiResponse = _mapper.Map<ExtendPoiResponse>(poi);

            //Store to Redis

            return new BaseResponse<ExtendPoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendPoiResponse
            };
        }


        /// <summary>
        /// Delete POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendPoiResponse>> DeletePoiById(string id)
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
            ExtendPoiResponse ExtendPoiResponse = _mapper.Map<ExtendPoiResponse>(poi);

            //Store Poi to redis

            return new BaseResponse<ExtendPoiResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendPoiResponse
            };
        }


        /// <summary>
        /// Get Pois By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ExtendPoiResponse>>> GetPoisByStatus(int status)
        {
            List<ExtendPoiResponse> poiList = null;

            //get Poi from database
            try
            {
                poiList = _mapper.Map<List<ExtendPoiResponse>>(
                    await _unitOfWork.Pois.FindListAsync(Poi => Poi.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.GetPoisByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendPoiResponse>
                    {
                        ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                        ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<ExtendPoiResponse>>
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
        public async Task<BaseResponse<List<ExtendPoiResponse>>> GetAllPoi()
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
                    new BaseResponse<ExtendPoiResponse>
                    {
                        ResultCode = (int)PoiStatus.POI_NOT_FOUND,
                        ResultMessage = PoiStatus.POI_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            List<ExtendPoiResponse> ExtendPoiResponses = _mapper.Map<List<ExtendPoiResponse>>(pois);

            return new BaseResponse<List<ExtendPoiResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendPoiResponses
            };
        }
    }
}
