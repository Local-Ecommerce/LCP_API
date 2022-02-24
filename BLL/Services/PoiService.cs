using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.POI;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
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
        public async Task<PoiResponse> CreatePoi(PoiRequest poiRequest)
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

                throw;
            }
            //Create Response
            PoiResponse poiResponse = _mapper.Map<PoiResponse>(poi);

            //Store Poi to Redis

            return _mapper.Map<PoiResponse>(poi);
        }


        /// <summary>
        /// Update Poi by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poiUpdateRequest"></param>
        /// <returns></returns>
        public async Task<PoiResponse> UpdatePoiById(string id, PoiUpdateRequest poiUpdateRequest)
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

                throw new EntityNotFoundException(typeof(Poi), id);
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

                throw;
            }

            return _mapper.Map<PoiResponse>(poi);
        }


        /// <summary>
        /// Delete POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PoiResponse> DeletePoiById(string id)
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

                throw new EntityNotFoundException(typeof(Poi), id);
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

                throw;
            }

            return _mapper.Map<PoiResponse>(poi);
        }


        /// <summary>
        /// Get Poi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="date"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<object> GetPoi(
            string id, string apartmentId,
            DateTime date, string title, string text, int?[] status,
            int? limit, int? page, string sort, string[] include)
        {
            PagingModel<Poi> poi;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }
            for (int i = 0; i < include.Length; i++)
            {
                include[i] = !string.IsNullOrEmpty(include[i]) ? _utilService.UpperCaseFirstLetter(include[i]) : null;
            }

            try
            {
                poi = await _unitOfWork.Pois
                    .GetPoi(id, apartmentId, date, title, text, status, limit, page, isAsc, propertyName, include);

                if (_utilService.IsNullOrEmpty(poi.List))
                    throw new EntityNotFoundException(typeof(Menu), "in the url");
            }
            catch (Exception e)
            {
                _logger.Error("[PoiService.GetPoi()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendPoiResponse>
            {
                List = _mapper.Map<List<ExtendPoiResponse>>(poi.List),
                Page = poi.Page,
                LastPage = poi.LastPage,
                Total = poi.Total,
            };
        }
    }
}
