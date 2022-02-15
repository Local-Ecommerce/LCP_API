﻿using BLL.Dtos.Menu;
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
        Task<MenuResponse> CreateMenu(MenuRequest menuRequest);


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
        Task<ExtendMenuResponse> GetMenuById(string menuId);


        /// <summary>
        /// Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuRequest"></param>
        /// <returns></returns>
        Task<MenuResponse> UpdateMenuById(string id, MenuUpdateRequest menuUpdateRequest);


        /// <summary>
        /// Delete Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MenuResponse> DeleteMenuById(string id);


        /// <summary>
        /// Add Products To Menu
        /// </summary>
        /// <param name="productInMenuRequest"></param>
        /// <returns></returns>
        Task<List<ExtendProductInMenuResponse>> AddProductsToMenu(string menuId,
            List<ProductInMenuRequest> productInMenuRequests);


        /// <summary>
        /// Get Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        Task<ExtendProductInMenuResponse> GetProductInMenuById(string productInMenuId);


        /// <summary>
        /// Get Products In Menu By Menu Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<List<ExtendProductInMenuResponse>> GetProductsInMenuByMenuId(string menuId);


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


        /// <summary>
        /// Get Menus By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<ExtendMenuResponse>> GetMenusByStatus(int status);


        /// <summary>
        /// Get All Menus
        /// </summary>
        /// <returns></returns>
        Task<List<ExtendMenuResponse>> GetAllMenus();


        /// <summary>
        /// Get Menus By Resident Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<List<ExtendMenuResponse>> GetMenusByResidentId(string residentId);
    }
}
