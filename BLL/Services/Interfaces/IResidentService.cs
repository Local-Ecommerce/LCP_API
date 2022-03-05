using BLL.Dtos.Resident;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IResidentService
    {
        /// <summary>
        /// Create Resident
        /// </summary>
        /// <param name="residentRequest"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<ResidentResponse> CreateResident(ResidentRequest residentRequest, string residentId);


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
        /// Get Resident
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="accountId"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<object> GetResident(
            string id, string apartmentId,
            string accountId, int? limit,
            int? page, string sort);


        /// <summary>
        /// Verify Resident
        /// </summary>
        /// <param name="id"></param>
        /// <param name="marketManagerId"></param>
        /// <param name="isApprove"></param>
        /// <returns></returns>
        Task<ExtendResidentResponse> VerifyResident(string id, string marketManagerId, bool isApprove);
    }
}
