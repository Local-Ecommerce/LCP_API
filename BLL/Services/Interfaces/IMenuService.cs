using BLL.Dtos;
using BLL.Dtos.Menu;
using BLL.Dtos.MerchantStore;
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
        /// Create Default Menu
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="storeName"></param>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        MenuResponse CreateDefaultMenu(string residentId, string storeName, string merchantStoreId);


        /// <summary>
        /// Get Menu By Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendMenuResponses>> GetMenuById(string menuId);


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
        Task<BaseResponse<List<ExtendProductInMenuResponse>>> AddProductsToMenu(string menuId,
            List<ProductInMenuRequest> productInMenuRequests);


        /// <summary>
        /// Get Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendProductInMenuResponse>> GetProductInMenuById(string productInMenuId);


        /// <summary>
        /// Get Products In Menu By Menu Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendProductInMenuResponse>>> GetProductsInMenuByMenuId(string menuId);


        /// <summary>
        /// Update Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <param name="productInMenuUpdateRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendProductInMenuResponse>> UpdateProductInMenuById(string productInMenuId,
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
        Task<BaseResponse<List<ExtendMenuResponses>>> GetMenusByStatus(int status);


        /// <summary>
        /// Get All Menus
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendMenuResponses>>> GetAllMenus();


        /// <summary>
        /// Get Menus By Resident Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendMenuResponses>>> GetMenusByResidentId(string residentId);
    }
}
