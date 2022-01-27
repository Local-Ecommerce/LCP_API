using DAL.Models;
using DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DAL.Constants;

namespace DAL.Repositories
{
    public class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get All Active Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apartment>> GetAllActiveApartment()
        {
            List<Apartment> apartments = await _context.Apartments
                                            .Where(ap => ap.Status == (int)ApartmentStatus.ACTIVE_APARTMENT)
                                            .OrderBy(ap => ap.Address)
                                            .ToListAsync();

            return apartments;
        }
        
        
        /// <summary>
        /// Get All Active Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apartment>> GetAllApartment()
        {
            List<Apartment> apartments = await _context.Apartments
                                            .Where(ap => ap.ApartmentId != null)
                                            .OrderBy(ap => ap.Address)
                                            .ToListAsync();

            return apartments;
        }


        /// <summary>
        /// Get Market Manager By Apartment Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Apartment> GetMarketManagerByApartmentId (string id)
        {
             Apartment apartment = await _context.Apartments
                                      .Where(ap => ap.ApartmentId.Equals(id))
                                      .Include(ap => ap.Residents
                                      .Where(res => res.Type.Equals(ResidentType.MARKET_MANAGER)))
                                      .FirstOrDefaultAsync();

            return apartment;
        }
    }
}
