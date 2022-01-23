using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private const int ACTIVE_PRODUCT_IN_MENU = 15001;

        public MenuRepository(LoichDBContext context) : base(context) { }

        /// <summary>
        /// Get All Menus Include Resident
        /// </summary>
        /// <returns></returns>
        public async Task<List<Menu>> GetAllMenusIncludeResident()
        {
            List<Menu> menus = await _context.Menus.Include(menus => menus.Resident)
                                                   .OrderByDescending(menus => menus.CreatedDate)
                                                   .ToListAsync();

            return menus;
        }


        /// <summary>
        /// Get Menu Include Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Menu> GetMenuIncludeResidentById(string id)
        {
            Menu menu = await _context.Menus
                                .Where(menus => menus.MenuId.Equals(id))
                                .Include(menus => menus.Resident)
                                .OrderByDescending(menus => menus.CreatedDate)
                                .FirstOrDefaultAsync();

            return menu;
        }


        /// <summary>
        /// Get Menu By Resident Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<List<Menu>> GetMenusByResidentId(string residentId)
        {
            List<Menu> menus = await _context.Menus
                                        .Where(menu => menu.ResidentId.Equals(residentId))
                                        .Include(menu => menu.StoreMenuDetails)
                                        .Include(menu => menu.ProductInMenus)
                                        .ThenInclude(pim => pim.Product)
                                        .Include(menu => menu.ProductInMenus)
                                        .ThenInclude(pim => pim.ProductCombination)
                                        .ToListAsync();

            return menus;
        }
    }
}