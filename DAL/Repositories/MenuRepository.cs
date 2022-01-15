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
        /// Get All Menus Include Merchant
        /// </summary>
        /// <returns></returns>
        public async Task<List<Menu>> GetAllMenus()
        {
            List<Menu> menus = await _context.Menus.OrderByDescending(menu => menu.CreatedDate)
                                                   .ToListAsync();

            return menus;
        }
    }
}