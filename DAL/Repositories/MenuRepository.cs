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
        /// Get Base Menu Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<string> GetBaseMenuId(string residentId)
        {
            Menu menu = await _context.Menus.Include(menu => menu.MerchantStore)
                             .Where(menu => menu.MerchantStore.ResidentId.Equals(residentId)
                             && menu.BaseMenu == true).FirstAsync();

            return menu.MenuId;
        }


        /// <summary>
        /// Get Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="residentId"></param>
        /// <param name="apartmentId"></param>
        /// <param name="isActive"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Menu>> GetMenu(
            string id = default, int?[] status = default, string residentId = default,
            string apartmentId = default, bool? isActive = default, int? limit = default,
            int? queryPage = default, bool? isAsc = default,
            string propertyName = default, string[] include = default)
        {
            IQueryable<Menu> query = _context.Menus.Where(menu => menu.MenuId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(menu => menu.MenuId.Equals(id));

            //filter by status
            if (status != null && status.Length != 0)
                query = query.Where(menu => status.Contains(menu.Status));

            //filter by residentId
            if (!string.IsNullOrEmpty(residentId))
                query = query.Include(menu => menu.MerchantStore)
                    .Where(menu => menu.MerchantStore.ResidentId.Equals(residentId));

            //filter by apartmentId
            if (!string.IsNullOrEmpty(apartmentId))
                query = query.Include(menu => menu.MerchantStore)
                             .Where(menu => menu.MerchantStore.ApartmentId.Equals(apartmentId));

            //filter by isActive
            if (isActive != null && isActive == true)
            {
                TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime vnTime = TimeZoneInfo.ConvertTime(DateTime.Now, vnZone);

                query = query.Where(menu => TimeSpan.Compare(vnTime.TimeOfDay, (TimeSpan)menu.TimeStart) > 0 &&
                            TimeSpan.Compare(vnTime.TimeOfDay, (TimeSpan)menu.TimeEnd) < 0);
            }


            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = (bool)isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //add include
            if (include != null && include.Length > 0)
            {
                foreach (var item in include)
                {
                    if (item.Equals("store"))
                        query = query.Include(menu => menu.MerchantStore);
                    if (item.Equals("product"))
                    {
                        query = query.Include(menu => menu.ProductInMenus
                                        .Where(pim => pim.Status.Equals((int)ProductInMenuStatus.ACTIVE_PRODUCT_IN_MENU)))
                                    .ThenInclude(pim => pim.Product)
                                    .ThenInclude(p => p.SystemCategory);
                    }
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