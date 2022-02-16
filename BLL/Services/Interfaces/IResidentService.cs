using BLL.Dtos.Resident;
using System.Collections.Generic;
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
        Task<ResidentResponse> CreateResident(ResidentRequest residentRequest);


        /// <summary>
        /// Get Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResidentResponse> GetResidentById(string id);


        /// <summary>
        /// Update Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentRequest"></param>
        /// <returns></returns>
        Task<ResidentResponse> UpdateResidentById(string id, ResidentUpdateRequest residentRequest);


        /// <summary>
        /// Delete Resident
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResidentResponse> DeleteResident(string id);


        /// <summary>
        /// Get Resident By Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<List<ResidentResponse>> GetResidentByApartmentId(string apartmentId);


        /// <summary>
        /// Get All Residents
        /// </summary>
        /// <returns></returns>
        Task<List<ResidentResponse>> GetAllResidents();


        /// <summary>
        /// Get Resident By Apartment Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<List<ResidentResponse>> GetResidentByAccountId(string accountId);
    }
}
