using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Menu;
using BLL.Dtos.ProductInMenu;
using BLL.Dtos.StoreMenuDetail;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IStoreMenuDetailService _storeMenuDetailService;
        private const string PREFIX = "MN_";

        public MenuService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IStoreMenuDetailService storeMenuDetailService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _storeMenuDetailService = storeMenuDetailService;
        }

        /// <summary>
        /// Create Menu
        /// </summary>
        /// <param name="menuRequest"></param>
        /// <returns></returns>
        public async Task<MenuResponse> CreateMenu(MenuRequest menuRequest)
        {
            Menu menu = _mapper.Map<Menu>(menuRequest);
            try
            {
                menu.MenuId = _utilService.CreateId(PREFIX);
                menu.CreatedDate = DateTime.Now;
                menu.UpdatedDate = DateTime.Now;
                menu.Status = (int)MenuStatus.ACTIVE_MENU;

                _unitOfWork.Menus.Add(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.CreateMenu()]: " + e.Message);
                throw;
            }
            return _mapper.Map<MenuResponse>(menu);
        }


        /// <summary>
        /// Get Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendMenuResponse> GetMenuById(string id)
        {
            Menu menu;
            try
            {
                menu = await _unitOfWork.Menus.GetMenuIncludeResidentById(id);
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetMenuById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), id);
            }
            return _mapper.Map<ExtendMenuResponse>(menu);
        }


        /// <summary>
        ///  Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuUpdateRequest"></param>
        /// <returns></returns>
        public async Task<MenuResponse> UpdateMenuById(string id, MenuUpdateRequest menuUpdateRequest)
        {
            Menu menu;
            try
            {
                menu = await _unitOfWork.Menus.FindAsync(m => m.MenuId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateMenuById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), id);
            }

            //Update Menu to DB
            try
            {
                menu = _mapper.Map(menuUpdateRequest, menu);
                menu.UpdatedDate = DateTime.Now;

                _unitOfWork.Menus.Update(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateMenuById()]: " + e.Message);
                throw;
            }

            return _mapper.Map<MenuResponse>(menu);
        }


        /// <summary>
        /// Delete Menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MenuResponse> DeleteMenuById(string id)
        {
            //Check id
            Menu menu;
            try
            {
                menu = await _unitOfWork.Menus.FindAsync(menu => menu.MenuId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteMenuById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), id);
            }

            //Delete Menu
            try
            {
                menu.Status = (int)MenuStatus.DELETED_MENU;
                menu.UpdatedDate = DateTime.Now;

                _unitOfWork.Menus.Update(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteMenuById()]: " + e.Message);
                throw;
            }

            return _mapper.Map<MenuResponse>(menu);
        }


        /// <summary>
        /// Add Products To Menu
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="productInMenuRequests"></param>
        /// <returns></returns>
        public async Task<List<ExtendProductInMenuResponse>> AddProductsToMenu(string menuId,
            List<ProductInMenuRequest> productInMenuRequests)
        {
            //biz rule
            List<ProductInMenu> productInMenus = _mapper.Map<List<ProductInMenu>>(productInMenuRequests);

            try
            {
                productInMenus.ForEach(productInMenu =>
                {
                    productInMenu.ProductInMenuId = _utilService.CreateId(PREFIX);
                    productInMenu.MenuId = menuId;
                    productInMenu.CreatedDate = DateTime.Now;
                    productInMenu.UpdatedDate = DateTime.Now;
                    productInMenu.Status = (int)ProductInMenuStatus.ACTIVE_PRODUCT_IN_MENU;

                    _unitOfWork.ProductInMenus.Add(productInMenu);
                });

                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.AddProductsToMenu()]: " + e.Message);
                throw;
            }

            return _mapper.Map<List<ExtendProductInMenuResponse>>(productInMenus);
        }


        /// <summary>
        /// Update Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <param name="productInMenuUpdateRequest"></param>
        /// <returns></returns>
        public async Task<ExtendProductInMenuResponse> UpdateProductInMenuById(string productInMenuId,
            ProductInMenuUpdateRequest productInMenuUpdateRequest)
        {
            ProductInMenu productInMenu;
            try
            {
                productInMenu = await _unitOfWork.ProductInMenus.FindAsync(p => p.ProductInMenuId.Equals(productInMenuId));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateProductInMenuById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductInMenu), productInMenuId);
            }

            //Update Product In Menu
            try
            {
                productInMenu.Status = productInMenuUpdateRequest.Status;
                productInMenu.Price = productInMenuUpdateRequest.Price;
                productInMenu.UpdatedDate = DateTime.Now;

                _unitOfWork.ProductInMenus.Update(productInMenu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateProductInMenuById()]: " + e.Message);
                throw;
            }

            return _mapper.Map<ExtendProductInMenuResponse>(productInMenu);
        }


        /// <summary>
        /// Delete Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        public async Task<string> DeleteProductInMenuById(string productInMenuId)
        {
            ProductInMenu productInMenu;
            try
            {
                productInMenu = await _unitOfWork.ProductInMenus.FindAsync(p =>
                                                                    p.ProductInMenuId.Equals(productInMenuId));

                _unitOfWork.ProductInMenus.Delete(productInMenu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteProductInMenuById()]: " + e.Message);
                throw;
            }

            return null;
        }


        /// <summary>
        /// Get Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        public async Task<ExtendProductInMenuResponse> GetProductInMenuById(string productInMenuId)
        {
            ProductInMenu productInMenu;

            try
            {
                productInMenu = await _unitOfWork.ProductInMenus.GetProductInMenusIncludeProductByProductInMenuId(productInMenuId);
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetProductInMenuById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductInMenu), productInMenuId);
            }

            return _mapper.Map<ExtendProductInMenuResponse>(productInMenu);
        }


        /// <summary>
        /// Get Products In Menu By Menu Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<List<ExtendProductInMenuResponse>> GetProductsInMenuByMenuId(string menuId)
        {
            List<ProductInMenu> productsInMenu;
            try
            {
                productsInMenu = await _unitOfWork.ProductInMenus.GetProductInMenusIncludeProductByMenuId(menuId);
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetProductsInMenuByMenuId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductInMenu), menuId);
            }

            return _mapper.Map<List<ExtendProductInMenuResponse>>(productsInMenu);
        }


        /// <summary>
        /// Get Menus By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<ExtendMenuResponse>> GetMenusByStatus(int status)
        {
            List<ExtendMenuResponse> menuList;

            //get Menu from database
            try
            {
                menuList = _mapper.Map<List<ExtendMenuResponse>>(
                    await _unitOfWork.Menus.FindListAsync(me => me.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetMenusByStatus()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), status);
            }
            return menuList;
        }


        /// <summary>
        /// Get All Menus
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExtendMenuResponse>> GetAllMenus()
        {
            List<Menu> menus;
            try
            {
                menus = await _unitOfWork.Menus.GetAllMenusIncludeResident();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetAllMenus()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), "all");
            }

            return _mapper.Map<List<ExtendMenuResponse>>(menus);
        }


        /// <summary>
        /// Get Menus By Resident Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<List<ExtendMenuResponse>> GetMenusByResidentId(string residentId)
        {
            List<ExtendMenuResponse> menuResponses;

            try
            {
                menuResponses = _mapper.Map<List<ExtendMenuResponse>>(
                    await _unitOfWork.Menus.GetMenusByResidentId(residentId));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetMenusByResidentId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Menu), residentId);

            }
            return menuResponses;
        }


        /// <summary>
        /// Create Default Menu
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="storeName"></param>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        public MenuResponse CreateDefaultMenu(string residentId, string storeName, string merchantStoreId)
        {
            Menu menu = new()
            {
                MenuId = _utilService.CreateId(PREFIX),
                MenuName = "Bảng giá của " + storeName,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = (int)MenuStatus.ACTIVE_MENU,
                ResidentId = residentId
            };

            _unitOfWork.Menus.Add(menu);

            ExtendMenuResponse menuResponse = _mapper.Map<ExtendMenuResponse>(menu);
            menuResponse.StoreMenuDetails = new Collection<StoreMenuDetailResponse>();
            menuResponse.StoreMenuDetails.Add(
                _storeMenuDetailService.CreateDefaultStoreMenuDetail(menu.MenuId, merchantStoreId));

            return menuResponse;
        }
    }
}
