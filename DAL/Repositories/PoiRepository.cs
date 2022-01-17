using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class PoiRepository : Repository<Poi>, IPoiRepository
    {
        public PoiRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get All Pois Include Resident And Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<Poi>> GetAllPoisIncludeApartmentAndResident()
        {
            List<Poi> pois = await _context.Pois
                                .Include(poi => poi.Apartment)
                                .Include(poi => poi.Resident)
                                .OrderByDescending(poi => poi.ReleaseDate)
                                .ToListAsync();

            return pois;
        }


        public async Task<Poi> GetPoiIncludeResidentByPoiId(string poiId)
        {
            Poi poi = await _context.Pois
                                .Where(poi => poi.PoiId.Equals(poiId))
                                .Include(poi => poi.Resident)
                                .OrderByDescending(poi => poi.ReleaseDate)
                                .FirstOrDefaultAsync();

            return poi;
        }
    }
}