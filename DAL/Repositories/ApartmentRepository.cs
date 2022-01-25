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
    }
}
