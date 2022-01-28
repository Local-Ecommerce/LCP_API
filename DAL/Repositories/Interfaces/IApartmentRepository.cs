using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IApartmentRepository : IRepository<Apartment>
    {

        /// <summary>
        /// Get All Active Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<Apartment>> GetAllActiveApartment();


        /// <summary>
        /// Get All Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<Apartment>> GetAllApartment();


        /// <summary>
        /// Get Market Manager By Apartment Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Apartment> GetMarketManagerByApartmentId(string id);
    }
}