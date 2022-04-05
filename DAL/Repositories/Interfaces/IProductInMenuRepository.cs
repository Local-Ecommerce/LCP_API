using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IProductInMenuRepository : IRepository<ProductInMenu>
    {
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
        Task<PagingModel<ProductInMenu>> GetProductInMenu(string id, string menuId,
            int? limit, int? queryPage, bool isAsc, string propertyName, string include);

        Task<List<ProductInMenu>> GetProductsInMenu(List<string> productInMenuIds);
    }
}