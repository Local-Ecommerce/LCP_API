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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PoiService : IPoiService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "Poi";

        public PoiService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
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
                poi.PoiId = _utilService.Create16Alphanumeric();
                poi.RealeaseDate = DateTime.Now;
                poi.Status = (int)PoiStatus.ACTIVE_POI;

                _unitOfWork.Repository<Poi>().Add(poi);

                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception e)
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
            _redisService.StoreToList(CACHE_KEY, poiResponse, new Predicate<PoiResponse>(a => a.PoiId == poiResponse.PoiId));

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
            poiResponse = _redisService.GetList<PoiResponse>(CACHE_KEY).Find(local => local.PoiId.Equals(id));

            //Get poi from DB
            if(poiResponse is null)
            {
                try
                {
                    Poi poi = await _unitOfWork.Repository<Poi>().FindAsync(local => local.PoiId.Equals(id));

                    poiResponse = _mapper.Map<PoiResponse>(poi);
                }
                catch(Exception e)
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
        /// Get Poi by Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<PoiResponse>>> GetPoiByApartmentId(string apartmentId)
        {
            List<PoiResponse> poiResponses = null;

            //Get Poi from Redis
            poiResponses = _redisService.GetList<PoiResponse>(CACHE_KEY).Where(poi => poi.AparmentId.Equals(apartmentId)).ToList();

            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(poiResponses))
            {
                try
                {
                    List<Poi> poi = await _unitOfWork.Repository<Poi>().FindListAsync(poi => poi.AparmentId.Equals(apartmentId));

                    poiResponses = _mapper.Map<List<PoiResponse>>(poi);
                }
                catch(Exception e)
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
    }
}
