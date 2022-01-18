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

        private const int THREE = 3;
        private const int ACTIVE_STATUS = 3001;

        /// <summary>
        /// Get All Level One And Two System Category
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemCategory>> GetAllLevelOneAndTwoSystemCategory()
        {
            List<SystemCategory> systemCategories = await _context.SystemCategories
                                                            .Where(sc => sc.CategoryLevel != THREE && sc.Status == ACTIVE_STATUS)
                                                            .OrderBy(sc => sc.CategoryLevel)
                                                            .ToListAsync();
            return systemCategories;
        }


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


        /// <summary>
        /// Get System Category By Id Include Inverse Belong To
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SystemCategory> GetSystemCategoryByIdIncludeInverseBelongTo(string id)
        {
            SystemCategory systemCategory = await _context.SystemCategories
                                                    .Where(sc => sc.SystemCategoryId.Equals(id))
                                                    .Include(sc => sc.InverseBelongToNavigation)
                                                    .ThenInclude(sc => sc.InverseBelongToNavigation)
                                                    .FirstOrDefaultAsync();

            return systemCategory;
        }
        
        
        /// <summary>
        /// Get System Category By Id Include One Level Down Inverse Belong To
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SystemCategory> GetSystemCategoryByIdIncludeOneLevelDownInverseBelongTo(string id)
        {
            SystemCategory systemCategory = await _context.SystemCategories
                                                    .Where(sc => sc.SystemCategoryId.Equals(id))
                                                    .Include(sc => sc.InverseBelongToNavigation)
                                                    .FirstOrDefaultAsync();

            return systemCategory;
        }
    }
}