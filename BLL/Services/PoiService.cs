﻿using AutoMapper;
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
        private readonly IFirebaseService _firebaseService;
        private const string PREFIX = "POI_";
        private const string TYPE = "POI";

        public PoiService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IFirebaseService firebaseService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _firebaseService = firebaseService;
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
                poi.ReleaseDate = _utilService.CurrentTimeInVietnam();
                poi.Status = (int)PoiStatus.ACTIVE_POI;
                poi.Image = _firebaseService
                        .UploadFilesToFirebase(poiRequest.Image, TYPE, poi.PoiId, "Image", 0)
                        .Result;

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
                string imageUrl = poi.Image;

                //update image
                if (poiUpdateRequest.Image != null && poiUpdateRequest.Image.Length > 0)
                {
                    //get the order of the last photo
                    int order = !string.IsNullOrEmpty(poi.Image) ? _utilService.LastImageNumber("Image", poi.Image) : 0;

                    //upload new image & remove image
                    foreach (var image in poiUpdateRequest.Image)
                    {
                        if (image.Contains("https://firebasestorage.googleapis.com/"))
                            imageUrl = imageUrl.Replace(image + "|", "");
                        else
                            imageUrl += _firebaseService
                                .UploadFilesToFirebase(new string[] { image }, TYPE, poi.PoiId, "Image", order).Result;
                    }
                }

                poi = _mapper.Map(poiUpdateRequest, poi);
                poi.Image = imageUrl;

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
        public async Task DeletePoiById(string id)
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
        }


        /// <summary>
        /// Get Poi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<object> GetPois(
            string id, string apartmentId, string type,
            DateTime date, string search, int?[] status,
            int? limit, int? page, string[] sort, string[] include)
        {
            PagingModel<Poi> poi;

            //sort
            List<string> sortProperty = null;
            if (!_utilService.IsNullOrEmpty(sort))
            {
                sortProperty = new();
                foreach (var param in sort)
                    if (!string.IsNullOrEmpty(param))
                    {
                        string direction = sort[0].ToString().Equals("+") ? "" : " descending";
                        sortProperty.Add(_utilService.UpperCaseFirstLetter(param[1..]) + direction);
                    }
            }

            //include
            for (int i = 0; i < include.Length; i++)
                include[i] = !string.IsNullOrEmpty(include[i]) ? _utilService.UpperCaseFirstLetter(include[i]) : null;

            try
            {
                poi = await _unitOfWork.Pois
                    .GetPoi(id, apartmentId, type, date, search, status, limit, page, sortProperty, include);
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
