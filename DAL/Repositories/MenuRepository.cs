using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Linq.Dynamic.Core;
using DAL.Constants;

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
        /// <param name="apartmentId"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Menu>> GetMenu(
            string id, int?[] status,
            string apartmentId, int? limit,
            int? queryPage, bool isAsc,
            string propertyName, string[] include)
        {
            IQueryable<Menu> query = _context.Menus.Where(menu => menu.MenuId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(menu => menu.MenuId.Equals(id));

            //filter by status
            if (status.Length != 0)
                query = query.Where(menu => status.Contains(menu.Status));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Include(menu => menu.MerchantStore)
                             .Where(menu => menu.MerchantStore.ApartmentId.Equals(apartmentId));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //add include
            if (include.Length > 0)
            {
                foreach (var item in include)
                {
                    if (item.Equals("store"))
                        query = query.Include(menu => menu.MerchantStore);
                    if (item.Equals("product"))
                        query = query.Include(menu => menu.ProductInMenus
                                        .Where(pim => pim.Status.Equals((int)ProductInMenuStatus.ACTIVE_PRODUCT_IN_MENU)))
                                    .ThenInclude(pim => pim.Product);
                }
            }

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Menu>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}