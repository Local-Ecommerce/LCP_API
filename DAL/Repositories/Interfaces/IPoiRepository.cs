using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IPoiRepository : IRepository<Poi>
    {
        /// <summary>
        /// Get All Pois Include Market Manager And Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<Poi>> GetAllPoisIncludeApartment();
    }
}