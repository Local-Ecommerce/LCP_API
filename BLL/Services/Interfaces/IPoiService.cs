using BLL.Dtos.POI;
using System;
using System.Collections.Generic;
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
        Task<PoiResponse> CreatePoi(PoiRequest poiRequest);

        /// <summary>
        /// Get POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExtendPoiResponse> GetPoiById(string id);

        /// <summary>
        /// Get POI by ReleaseDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<List<ExtendPoiResponse>> GetPoiByReleaseDate(DateTime date);

        /// <summary>
        /// Get POI by apartmentId
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<List<ExtendPoiResponse>> GetPoiByApartmentId(string apartmentId);


        /// <summary>
        /// Update POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poiUpdateRequest"></param>
        /// <returns></returns>
        Task<PoiResponse> UpdatePoiById(string id, PoiUpdateRequest poiUpdateRequest);

        /// <summary>
        /// Delte POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PoiResponse> DeletePoiById(string id);


        /// <summary>
        /// Get Pois By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<ExtendPoiResponse>> GetPoisByStatus(int status);

        /// <summary>
        /// Get All POI
        /// </summary>
        /// <returns></returns>
        Task<List<ExtendPoiResponse>> GetAllPoi();
    }
}
