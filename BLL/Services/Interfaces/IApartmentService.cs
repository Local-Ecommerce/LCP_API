using BLL.Dtos.Apartment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IApartmentService
    {

        /// <summary>
        /// Create Apartment
        /// </summary>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        Task<ApartmentResponse> CreateApartment(ApartmentRequest apartmentRequest);


        /// <summary>
        /// Update Apartment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        Task<ApartmentResponse> UpdateApartmentById(string id, ApartmentRequest apartmentRequest);


        /// <summary>
        /// Delete Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApartmentResponse> DeleteApartment(string id);


        /// <summary>
        /// Get Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetApartment(string id, int[] status, int? limit, int? page, string sort, string include);
    }
}
