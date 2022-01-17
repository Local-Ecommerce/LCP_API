using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IPoiRepository : IRepository<Poi>
    {
        /// <summary>
        /// Get All Pois Include Resident And Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<Poi>> GetAllPoisIncludeApartmentAndResident();


        /// <summary>
        /// Get Poi Include Resident By Poi Id
        /// </summary>
        /// <param name="poiId"></param>
        /// <returns></returns>
        Task<Poi> GetPoiIncludeResidentByPoiId(string poiId);
    }
}