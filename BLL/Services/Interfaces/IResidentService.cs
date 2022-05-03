using BLL.Dtos.Resident;
using DAL.Models;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IResidentService
    {
        /// <summary>
        /// Create Merchant
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<Resident> CreateMerchant(string residentId);


        /// <summary>
        /// Create Guest
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="apartmentId"></param>
        /// <param name="marketManagerId"></param>
        /// <returns>residentId</returns>
        Task<Resident> CreateGuest(ResidentGuest guest, string apartmentId, string marketManagerId);


        /// <summary>
        /// Update Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentRequest"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<ResidentResponse> UpdateResidentById(string id, ResidentUpdateRequest residentRequest, string role);


        /// <summary>
        /// Delete Resident
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteResident(string id);


        /// <summary>
        /// Get Resident
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="accountId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<object> GetResident(
            string id, string apartmentId, string phoneNumber,
            string accountId, int?[] status, string type, int? limit,
            int? page, string sort);


        /// <summary>
        /// Update Resident Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="managerId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<ExtendResidentResponse> UpdateResidentStatus(string id, int status, string managerId, string role);
    }
}
