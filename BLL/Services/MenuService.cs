﻿using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.Menu;
using BLL.Dtos.ProductInMenu;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "Menu";

        public MenuService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
        }

        /// <summary>
        /// Create Menu
        /// </summary>
        /// <param name="MenuRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MenuResponse>> CreateMenu(MenuRequest menuRequest)
        {
            Menu menu = _mapper.Map<Menu>(menuRequest);

            try
            {
                menu.MenuId = _utilService.Create16Alphanumeric();
                menu.CreatedDate = DateTime.Now;
                menu.UpdatedDate = DateTime.Now;
                menu.Status = (int)MenuStatus.ACTIVE_MENU;

                _unitOfWork.Repository<Menu>().Add(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.CreateMenu()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<MenuResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }
            //Create Response
            MenuResponse menuResponse = _mapper.Map<MenuResponse>(menu);

            //Store Menu to Redis
            _redisService.StoreToList(CACHE_KEY, menuResponse,
                new Predicate<MenuResponse>(menu => menu.MenuId == menuResponse.MenuId));

            return new BaseResponse<MenuResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = menuResponse
            };
        }


        /// <summary>
        /// Get Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MenuResponse>> GetMenuById(string id)
        {
            MenuResponse menuReponse = null;
            //Get Menu from Redis
            //menuReponse = _redisService.GetList<MenuResponse>(CACHE_KEY).Find(menu => menu.MenuId.Equals(id));

            //Get Menu from DB
            if (menuReponse is null)
            {
                try
                {
                    Menu Menu = await _unitOfWork.Repository<Menu>().FindAsync(menu => menu.MenuId.Equals(id));

                    menuReponse = _mapper.Map<MenuResponse>(Menu);
                }
                catch (Exception e)
                {
                    _logger.Error("[MenuService.GetMenuById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Menu>
                    {
                        ResultCode = (int)MenuStatus.MENU_NOT_FOUND,
                        ResultMessage = MenuStatus.MENU_NOT_FOUND.ToString(),
                        Data = default
                    });
                }
            }

            return new BaseResponse<MenuResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = menuReponse
            };
        }


        /// <summary>
        /// Get Menu By Release Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<MenuResponse>>> GetMenusByMerchantId(string merchantId)
        {
            List<MenuResponse> menuResponses = null;

            //Get Menu from Redis
            menuResponses = _redisService.GetList<MenuResponse>(CACHE_KEY)
                .Where(menu => menu.MerchantId.Equals(merchantId))
                .ToList();

            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(menuResponses))
            {
                try
                {
                    List<Menu> Menu = await _unitOfWork.Repository<Menu>().FindListAsync(menu => menu.MerchantId.Equals(merchantId));

                    menuResponses = _mapper.Map<List<MenuResponse>>(Menu);
                }
                catch (Exception e)
                {
                    _logger.Error("[MenuService.GetMenuByMerchantId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Menu>
                    {
                        ResultCode = (int)MenuStatus.MENU_NOT_FOUND,
                        ResultMessage = MenuStatus.MENU_NOT_FOUND.ToString(),
                        Data = default
                    });
                }
            }

            return new BaseResponse<List<MenuResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = menuResponses
            };
        }


        /// <summary>
        ///  Update Menu By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MenuRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MenuResponse>> UpdateMenuById(string id, MenuUpdateRequest menuUpdateRequest)
        {
            Menu menu;
            try
            {
                menu = await _unitOfWork.Repository<Menu>().FindAsync(m => m.MenuId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Menu>
                {
                    ResultCode = (int)MenuStatus.MENU_NOT_FOUND,
                    ResultMessage = MenuStatus.MENU_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update Menu to DB
            try
            {
                menu = _mapper.Map(menuUpdateRequest, menu);
                menu.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Menu>().Update(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Menu>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            MenuResponse menuResponse = _mapper.Map<MenuResponse>(menu);

            //Store to Redis
            _redisService.StoreToList(CACHE_KEY, menuResponse, new Predicate<MenuResponse>(m => m.MenuId == menuResponse.MenuId));

            return new BaseResponse<MenuResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = menuResponse
            };
        }


        /// <summary>
        /// Delete Menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<MenuResponse>> DeleteMenuById(string id)
        {
            //Check id
            Menu menu;
            try
            {
                menu = await _unitOfWork.Repository<Menu>().FindAsync(menu => menu.MenuId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Menu>
                {
                    ResultCode = (int)MenuStatus.MENU_NOT_FOUND,
                    ResultMessage = MenuStatus.MENU_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Delete Menu
            try
            {
                menu.Status = (int)MenuStatus.INACTIVE_MENU;
                menu.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Menu>().Update(menu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Menu>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            MenuResponse menuResponse = _mapper.Map<MenuResponse>(menu);

            //Store Menu to Redis
            _redisService.StoreToList<MenuResponse>(CACHE_KEY, menuResponse,
                new Predicate<MenuResponse>(m => m.MenuId == menuResponse.MenuId));

            return new BaseResponse<MenuResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = menuResponse
            };
        }


        /// <summary>
        /// Add Products To Menu
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="productInMenuRequests"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ProductInMenuResponse>>> AddProductsToMenu(string menuId,
            List<ProductInMenuRequest> productInMenuRequests)
        {
            //biz rule
            List<ProductInMenu> productInMenus = _mapper.Map<List<ProductInMenu>>(productInMenuRequests);

            try
            {
                productInMenus.ForEach(productInMenu =>
                {
                    productInMenu.ProductInMenuId = _utilService.Create16Alphanumeric();
                    productInMenu.MenuId = menuId;
                    productInMenu.CreatedDate = DateTime.Now;
                    productInMenu.Status = (int)ProductInMenuStatus.ACTIVE_PRODUCT_IN_MENU;

                    _unitOfWork.Repository<ProductInMenu>().Add(productInMenu);
                });

                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.AddProductsToMenu()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ProductInMenuResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //create response
            List<ProductInMenuResponse> productInMenuResponses = _mapper.Map<List<ProductInMenuResponse>>(productInMenus);

            return new BaseResponse<List<ProductInMenuResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productInMenuResponses
            };
        }


        /// <summary>
        /// Update Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <param name="productInMenuUpdateRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ProductInMenuResponse>> UpdateProductInMenuById(string productInMenuId,
            ProductInMenuUpdateRequest productInMenuUpdateRequest)
        {
            ProductInMenu productInMenu;
            try
            {
                productInMenu = await _unitOfWork.Repository<ProductInMenu>().FindAsync(p => p.ProductInMenuId.Equals(productInMenuId));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateProductInMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Menu>
                {
                    ResultCode = (int)MenuStatus.MENU_NOT_FOUND,
                    ResultMessage = MenuStatus.MENU_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update Product In Menu to DB
            try
            {
                productInMenu.Status = productInMenuUpdateRequest.Status;
                productInMenu.Price = productInMenuUpdateRequest.Price;

                _unitOfWork.Repository<ProductInMenu>().Update(productInMenu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateProductInMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ProductInMenu>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            ProductInMenuResponse productInMenuResponse = _mapper.Map<ProductInMenuResponse>(productInMenu);

            return new BaseResponse<ProductInMenuResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productInMenuResponse
            };
        }


        /// <summary>
        /// Delete Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<string>> DeleteProductInMenuById(string productInMenuId)
        {
            ProductInMenu productInMenu;
            try
            {
                productInMenu = await _unitOfWork.Repository<ProductInMenu>().FindAsync(p =>
                                                                    p.ProductInMenuId.Equals(productInMenuId));

                _unitOfWork.Repository<ProductInMenu>().Delete(productInMenu);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.DeleteProductInMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ProductInMenu>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<string>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productInMenu.MenuId.ToString()
            };
        }


        /// <summary>
        /// Get Product In Menu By Id
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ProductInMenuResponse>> GetProductInMenuById(string productInMenuId)
        {
            ProductInMenu productInMenu;
            try
            {
                productInMenu = await _unitOfWork.Repository<ProductInMenu>().FindAsync(p =>
                                                                    p.ProductInMenuId.Equals(productInMenuId));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetProductInMenuById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ProductInMenu>
                {
                    ResultCode = (int)ProductInMenuStatus.PRODUCT_IN_MENU_NOT_FOUND,
                    ResultMessage = ProductInMenuStatus.PRODUCT_IN_MENU_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //create response
            ProductInMenuResponse productInMenuResponse = _mapper.Map<ProductInMenuResponse>(productInMenu);

            return new BaseResponse<ProductInMenuResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productInMenuResponse
            };
        }


        /// <summary>
        /// Get Products In Menu By Menu Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ProductInMenuResponse>>> GetProductsInMenuByMenuId(string menuId)
        {
            List<ProductInMenu> productsInMenu;
            try
            {
                productsInMenu = await _unitOfWork.Repository<ProductInMenu>().FindListAsync(p =>
                                                                    p.MenuId.Equals(menuId));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetProductsInMenuByMenuId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<ProductInMenu>
                {
                    ResultCode = (int)ProductInMenuStatus.PRODUCT_IN_MENU_NOT_FOUND,
                    ResultMessage = ProductInMenuStatus.PRODUCT_IN_MENU_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //create response
            List<ProductInMenuResponse> productInMenuResponses = _mapper.Map<List<ProductInMenuResponse>>(productsInMenu);

            return new BaseResponse<List<ProductInMenuResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productInMenuResponses
            };
        }
    }
}