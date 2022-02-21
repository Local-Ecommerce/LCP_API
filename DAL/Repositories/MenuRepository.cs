using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Linq.Dynamic.Core;

namespace DAL.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {

        public MenuRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="residentId"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Menu>> GetMenu(
            string id, int[] status,
            string residentId, int? limit,
            int? queryPage, bool isAsc,
            string propertyName, string include)
        {
            IQueryable<Menu> query = _context.Menus.Where(menu => menu.MenuId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(menu => menu.MenuId.Equals(id));

            //filter by status
            if (status.Length != 0)
                query = query.Where(menu => Array.IndexOf(status, menu.Status) > -1);

            //filter by residentId
            if (!string.IsNullOrEmpty(residentId))
                query = query.Where(menu => menu.ResidentId.Equals(residentId));

            //add include
            if (!string.IsNullOrEmpty(include))
                if (include.Equals(nameof(Menu.Resident)))
                    query = query.Include(menu => menu.Resident);

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(10);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Menu>
            {
                List = await query.Take(perPage).Skip((page - 1) * perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}