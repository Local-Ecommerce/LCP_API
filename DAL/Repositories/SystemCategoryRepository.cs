using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Constants;
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
        /// <param name="merchantId"></param>
        /// <param name="status"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<SystemCategory>> GetSystemCategory(
            string id, string merchantId,
            int?[] status, string search,
            int? limit, int? queryPage, bool isAsc,
            string propertyName, string include)
        {
            IQueryable<SystemCategory> query = _context.SystemCategories.Where(sc => sc.SystemCategoryId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(sc => sc.SystemCategoryId.Equals(id));

            //filter by merchant Id
            if (!string.IsNullOrEmpty(merchantId))
                query = query.Include(sc => sc.Products.Where(p => p.ResidentId.Equals(merchantId)));

            //filter by status
            if (status != null && status.Length != 0)
                query = query.Where(sc => status.Contains(sc.Status));

            //filter by search
            if (!string.IsNullOrEmpty(search))
                query = query.Where(sc => sc.SysCategoryName.ToLower().Contains(search.ToLower()));

            //filter by include
            if (!string.IsNullOrEmpty(include) && include.Equals("parent"))
                query = query.Include(sc => sc.BelongToNavigation);
            else
                query = query.Where(sc => string.IsNullOrEmpty(sc.BelongTo))
                                .Include(sc => sc.InverseBelongToNavigation
                                    .Where(sc => sc.Status.Equals((int)SystemCategoryStatus.ACTIVE_SYSTEM_CATEGORY)))
                                .ThenInclude(sci => sci.InverseBelongToNavigation
                                        .Where(sc => sc.Status.Equals((int)SystemCategoryStatus.ACTIVE_SYSTEM_CATEGORY)));


            //sort
            if (!string.IsNullOrEmpty(propertyName))
                query = isAsc ? query.OrderBy(c => propertyName) : query.OrderByDescending(c => propertyName);

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
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