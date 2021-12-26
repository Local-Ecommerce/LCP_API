using BLL.Dtos;
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
        Task<BaseResponse<PoiResponse>> GetPoiById(string id);

        /// <summary>
        /// Get POI by apartmentId
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<PoiResponse>>> GetPoiByApartmentId(string apartmentId);

        /// <summary>
        /// Get POI by marketManagerId
        /// </summary>
        /// <param name="marketManagerId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<PoiResponse>>> GetPoiByMarketManagerId(string marketManagerId);

        /// <summary>
        /// Update POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poiRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<PoiResponse>> UpdatePoiById(string id, PoiRequest poiRequest);

        /// <summary>
        /// Delte POI by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<PoiResponse>> DeletePoiById(string id);
    }
}
