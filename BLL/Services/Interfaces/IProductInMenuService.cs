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
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetProductsInMenu(
            string id, string menuId,
            int? limit, int? page, string sort, string include);


        /// <summary>
        /// Update Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <param name="productInMenuUpdateRequest"></param>
        /// <returns></returns>
        Task<ExtendProductInMenuResponse> UpdateProductInMenuById(string productInMenuId,
            ProductInMenuUpdateRequest productInMenuUpdateRequest);


        /// <summary>
        /// Delete Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        Task<string> DeleteProductInMenuById(string productInMenuId);
    }
}
