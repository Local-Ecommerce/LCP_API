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
        /// Get Apartment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApartmentResponse> GetApartmentById(string id);


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
        /// Get Apartment By Addess
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<ApartmentResponse> GetApartmentByAddress(string address);


        /// <summary>
        /// Get Apartment By Status
        /// </summary>
        /// <returns></returns>
        Task<List<ApartmentResponse>> GetApartmentsByStatus(int status);


        /// <summary>
        /// Get Apartment For Auto Complete
        /// </summary>
        /// <returns></returns>
        Task<List<ApartmentResponse>> GetApartmentForAutoComplete();
        
        
        /// <summary>
        /// Get Apartment For Auto Complete
        /// </summary>
        /// <returns></returns>
        Task<List<ApartmentResponse>> GetAllApartments();


        /// <summary>
        /// Get Market Manager By Apartment Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExtendApartmentResponse> GetMarketManagerByApartmentId(string id);
    }
}
