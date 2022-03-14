using BLL.Dtos.POI;
using System;
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
        /// Get Poi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="date"></param>        
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetPois(
            string id, string apartmentId,
            DateTime date, string search,
            int?[] status, int? limit, int? page,
            string sort, string[] include);


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
        Task DeletePoiById(string id);
    }
}
