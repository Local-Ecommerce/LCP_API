using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(LoichDBContext context) : base(context) { }

        /// <summary>
        /// Get All Menus Include Merchant
        /// </summary>
        /// <returns></returns>
        public async Task<List<Menu>> GetAllMenusIncludeMerchant()
        {
            List<Menu> menus = await _context.Menus.Include(menu => menu.Merchant).ToListAsync();

            return menus;
        }
    }
}