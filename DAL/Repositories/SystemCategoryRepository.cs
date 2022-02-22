using System;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        /// Get System Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public async Task<PagingModel<SystemCategory>> GetSystemCategory(
            string id, int? limit,
            int? queryPage, bool isAsc,
            string propertyName)
        {
            IQueryable<SystemCategory> query = _context.SystemCategories.Where(sc => sc.SystemCategoryId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(sc => sc.SystemCategoryId.Equals(id));
            else
                query = query.Where(sc => sc.BelongTo == null)
                             .Include(sc => sc.InverseBelongToNavigation)
                             .ThenInclude(sc => sc.InverseBelongToNavigation);

            //sort
            if (!string.IsNullOrEmpty(propertyName))
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");

            //paging
            int perPage = limit.GetValueOrDefault(10);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<SystemCategory>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}