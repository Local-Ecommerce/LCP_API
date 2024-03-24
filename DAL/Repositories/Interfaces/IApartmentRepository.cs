using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces {
	public interface IApartmentRepository : IRepository<Apartment> {
		/// <summary>
		/// Get Apartment
		/// </summary>
		/// <param name="apartment"></param>
		/// <returns></returns>
		Task<PagingModel<Apartment>> GetApartment(ApartmentPagingRequest apartment);
	}
}