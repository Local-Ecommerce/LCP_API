using BLL.Dtos;
using BLL.Dtos.Resident;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IResidentService
    {
        /// <summary>
        /// Create Resident
        /// </summary>
        /// <param name="residentRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ResidentResponse>> CreateResident(ResidentRequest residentRequest);


        /// <summary>
        /// Get Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ResidentResponse>> GetResidentById(string id);


        /// <summary>
        /// Update Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ResidentResponse>> UpdateResidentById(string id, ResidentUpdateRequest residentRequest);


        /// <summary>
        /// Delete Resident
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ResidentResponse>> DeleteResident(string id);


        /// <summary>
        /// Get Resident By Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ResidentResponse>>> GetResidentByApartmentId(string apartmentId);


        /// <summary>
        /// Get All Residents
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<ResidentResponse>>> GetAllResidents();


        /// <summary>
        /// Get Resident By Apartment Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ResidentResponse>>> GetResidentByAccountId(string accountId);
    }
}
