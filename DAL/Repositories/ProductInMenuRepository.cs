using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public class ProductInMenuRepository : Repository<ProductInMenu>, IProductInMenuRepository
    {
        public ProductInMenuRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Product In Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuId"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<ProductInMenu>> GetProductInMenu(string id, string menuId,
            int? limit, int? queryPage, bool isAsc,
            string propertyName, string include)
        {
            IQueryable<ProductInMenu> query = _context.ProductInMenus.Where(pim => pim.ProductInMenuId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(pim => pim.MenuId.Equals(id));

            //filter by menuId
            if (!string.IsNullOrEmpty(menuId))
                query = query.Where(pim => pim.MenuId.Equals(menuId));

            //add include
            if (!string.IsNullOrEmpty(include))
                if (include.Equals(nameof(ProductInMenu.Product)))
                    query = query.Include(pim => pim.Product);

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<ProductInMenu>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }

        public async Task<List<ProductInMenu>> GetProductsInMenu(List<string> productInMenuIds)
        {
            return await _context.ProductInMenus.Where(pim => productInMenuIds.Contains(pim.ProductInMenuId))
                            .Include(pim => pim.Product)
                            .ThenInclude(p => p.InverseBelongToNavigation)
                            .ToListAsync();
        }
    }
}