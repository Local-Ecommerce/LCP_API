using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductInMenuRepository : Repository<ProductInMenu>, IProductInMenuRepository
    {
        public ProductInMenuRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Product In Menus Include Product By Product In Menu Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        public async Task<ProductInMenu> GetProductInMenusIncludeProductByProductInMenuId(string productInMenuId)
        {
            ProductInMenu productInMenu = await _context.ProductInMenus
                                            .Where(pim => pim.ProductInMenuId.Equals(productInMenuId))
                                            .Include(pim => pim.Product)
                                            .FirstOrDefaultAsync();

            return productInMenu;
        }
    }
}