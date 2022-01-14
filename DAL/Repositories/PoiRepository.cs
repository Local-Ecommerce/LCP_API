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
        /// Get All Pois Include Market Manager And Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<Poi>> GetAllPoisIncludeMarketManagerAndApartment()
        {
            List<Poi> pois = await _context.Pois
                                .Include(poi => poi.MarketManager)
                                .Include(poi => poi.Apartment)
                                .OrderByDescending(poi => poi.ReleaseDate)
                                .ToListAsync();

            return pois;
        }
    }
}