﻿using BLL.Dtos;
using BLL.Dtos.POI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IPoiService
    {
        /// <summary>
        /// Create POI
        /// </summary>
        /// <param name="poiRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<PoiResponse>> CreatePoi(PoiRequest poiRequest);

        /// <summary>
        /// Get POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendPoiResponse>> GetPoiById(string id);

        /// <summary>
        /// Get POI by ReleaseDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendPoiResponse>>> GetPoiByReleaseDate(DateTime date);

        /// <summary>
        /// Get POI by apartmentId
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendPoiResponse>>> GetPoiByApartmentId(string apartmentId);


        /// <summary>
        /// Update POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poiUpdateRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<PoiResponse>> UpdatePoiById(string id, PoiUpdateRequest poiUpdateRequest);

        /// <summary>
        /// Delte POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<PoiResponse>> DeletePoiById(string id);


        /// <summary>
        /// Get Pois By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendPoiResponse>>> GetPoisByStatus(int status);

        /// <summary>
        /// Get All POI
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendPoiResponse>>> GetAllPoi();
    }
}
