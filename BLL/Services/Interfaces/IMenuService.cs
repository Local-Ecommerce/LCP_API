using BLL.Dtos;
using BLL.Dtos.Menu;
using BLL.Dtos.ProductInMenu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMenuService
    {
        /// <summary>
        /// Create Menu
        /// </summary>
        /// <param name="menuRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MenuResponse>> CreateMenu(MenuRequest menuRequest);


        /// <summary>
        /// Get Menu By Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<BaseResponse<MenuResponse>> GetMenuById(string menuId);


        /// <summary>
        /// Get Menus By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<MenuResponse>>> GetMenusByMerchantId(string merchantId);


        /// <summary>
        /// Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<MenuResponse>> UpdateMenuById(string id, MenuUpdateRequest menuUpdateRequest);


        /// <summary>
        /// Delete Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<MenuResponse>> DeleteMenuById(string id);


        /// <summary>
        /// Add Products To Menu
        /// </summary>
        /// <param name="productInMenuRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ProductInMenuResponse>>> AddProductsToMenu(string menuId, 
            List<ProductInMenuRequest> productInMenuRequests);


        /// <summary>
        /// Get Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductInMenuResponse>> GetProductInMenuById(string productInMenuId);


        /// <summary>
        /// Get Products In Menu By Menu Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ProductInMenuResponse>>> GetProductsInMenuByMenuId(string menuId);


        /// <summary>
        /// Update Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <param name="productInMenuUpdateRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductInMenuResponse>> UpdateProductInMenuById(string productInMenuId,
            ProductInMenuUpdateRequest productInMenuUpdateRequest);


        /// <summary>
        /// Delete Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        Task<BaseResponse<string>> DeleteProductInMenuById(string productInMenuId);


        /// <summary>
        /// Get Menus By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<MenuResponse>>> GetMenusByStatus(int status);
    }
}
