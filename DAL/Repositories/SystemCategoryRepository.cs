using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SystemCategoryRepository : Repository<SystemCategory>, ISystemCategoryRepository
    {
        public SystemCategoryRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get All System Category Include Inverse Belong To
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemCategory>> GetAllSystemCategoryIncludeInverseBelongTo()
        {
            List<SystemCategory> systemCategories = await _context.SystemCategories
                                                            .Where(sc => sc.BelongTo == null)
                                                            .Include(sc => sc.InverseBelongToNavigation)
                                                            .ThenInclude(sc => sc.InverseBelongToNavigation)
                                                            .ToListAsync();

            return systemCategories;
        }
    }
}