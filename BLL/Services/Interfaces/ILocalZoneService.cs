using BLL.Dtos;
using BLL.Dtos.LocalZone;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ILocalZoneService
    {

        /// <summary>
        /// Create LocalZone
        /// </summary>
        /// <param name="localZoneRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<LocalZoneResponse>> CreateLocalZone(LocalZoneRequest localZoneRequest);


        /// <summary>
        /// Get LocalZone By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<LocalZoneResponse>> GetLocalZoneById(string id);


        /// <summary>
        /// Update LocalZone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="localZoneRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<LocalZoneResponse>> UpdateLocalZoneById(string id, LocalZoneRequest localZoneRequest);


        /// <summary>
        /// Delete LocalZone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<LocalZoneResponse>> DeleteLocalZone(string id);


        /// <summary>
        /// Store LocalZone To Redis
        /// </summary>
        /// <param name="localZone"></param>
        void StoreLocalZoneToRedis(LocalZoneResponse localZone);
    }
}
