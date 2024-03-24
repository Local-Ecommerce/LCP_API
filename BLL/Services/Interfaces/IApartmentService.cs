using BLL.Dtos.Apartment;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces {
	public interface IApartmentService {

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
		Task DeleteApartment(string id);


		/// <summary>
		/// Get Apartments
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<object> GetApartments(GetApartmentRequest request);
	}
}
