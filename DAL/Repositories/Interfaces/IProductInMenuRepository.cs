using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IProductInMenuRepository : IRepository<ProductInMenu>
    {
        /// <summary>
        /// Get Product In Menus Include Product By Product In Menu Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        Task<ProductInMenu> GetProductInMenusIncludeProductByProductInMenuId(string productInMenuId);

    }
}