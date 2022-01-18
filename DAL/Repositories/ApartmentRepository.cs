using DAL.Models;
using DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(LoichDBContext context) : base(context) { }

        private const int ACTIVE_STATUS = 4001;


        /// <summary>
        /// Get All Active Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<Apartment>> GetAllActiveApartment()
        {
            List<Apartment> apartments = await _context.Apartments
                                            .Where(ap => ap.Status == ACTIVE_STATUS)
                                            .OrderBy(ap => ap.Address)
                                            .ToListAsync();

            return apartments;
        }
    }
}
