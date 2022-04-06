using BLL.Dtos.ProductInMenu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductInMenuService
    {
        /// <summary>
        /// Add Products To Menu
        /// </summary>
        /// <param name="productInMenuRequest"></param>
        /// <returns></returns>
        Task<List<ExtendProductInMenuResponse>> AddProductsToMenu(string menuId,
            List<ProductInMenuRequest> productInMenuRequests);


        /// <summary>
        /// GetProductsInMenu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetProductsInMenu(
            string id, string menuId, int?[] status,
            int? limit, int? page, string sort, string include);


        /// <summary>
        /// Update Products In Menu By Id
        /// </summary>
        /// <param name="productInMenuUpdateRequests"></param>
        /// <returns></returns>
        Task<List<ExtendProductInMenuResponse>> UpdateProductsInMenu(
            ListProductInMenuUpdateRequest productInMenuUpdateRequests);


        /// <summary>
        /// Delete Product In Menu By Ids
        /// </summary>
        /// <param name="productInMenuIds"></param>
        /// <returns></returns>
        Task DeleteProductInMenu(List<string> productInMenuIds);
    }
}
