using BLL.Dtos;
using BLL.Dtos.Apartment;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IApartmentService
    {

        /// <summary>
        /// Create Apartment
        /// </summary>
        /// <param name="ApartmentRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ApartmentResponse>> CreateApartment(ApartmentRequest apartmentRequest);


        /// <summary>
        /// Get Apartment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ApartmentResponse>> GetApartmentById(string id);


        /// <summary>
        /// Update Apartment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ApartmentResponse>> UpdateApartmentById(string id, ApartmentRequest apartmentRequest);


        /// <summary>
        /// Delete Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ApartmentResponse>> DeleteApartment(string id);
    }
}
