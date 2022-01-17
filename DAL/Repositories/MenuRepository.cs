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



        public async Task<Menu> GetMenuIncludeResidentById(string id)
        {
            Menu menu = await _context.Menus
                                .Where(menus => menus.MenuId.Equals(id))
                                .Include(menus => menus.Resident)
                                .OrderByDescending(menus => menus.CreatedDate)
                                .FirstOrDefaultAsync();

            return menu;
        }
    }
}