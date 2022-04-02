using AutoMapper;
using BLL.Dtos.Exception;
using BLL.Dtos.ProductInMenu;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductInMenuService : IProductInMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PIM_";

        public ProductInMenuService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
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
            List<ProductInMenu> productInMenus = _mapper.Map<List<ProductInMenu>>(productInMenuRequests);

            try
            {
                productInMenus.ForEach(productInMenu =>
                {
                    productInMenu.ProductInMenuId = _utilService.CreateId(PREFIX);
                    productInMenu.MenuId = menuId;
                    productInMenu.CreatedDate = _utilService.CurrentTimeInVietnam();
                    productInMenu.UpdatedDate = _utilService.CurrentTimeInVietnam();
                    productInMenu.Status = (int)ProductInMenuStatus.ACTIVE_PRODUCT_IN_MENU;

                    _unitOfWork.ProductInMenus.Add(productInMenu);
                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductInMenuService.AddProductsToMenu()]: " + e.Message);
                throw;
            }

            return _mapper.Map<List<ExtendProductInMenuResponse>>(productInMenus);
        }


        /// <summary>
        /// Delete Product In Menu By Ids
        /// </summary>
        /// <param name="productInMenuIdz"></param>
        /// <returns></returns>
        public async Task DeleteProductInMenu(List<string> productInMenuIds)
        {
            List<ProductInMenu> productInMenus;
            try
            {
                productInMenus = await _unitOfWork.ProductInMenus.FindListAsync(p => productInMenuIds.Contains(p.ProductInMenuId));
                foreach (var productInMenu in productInMenus)
                {
                    productInMenu.Status = (int)ProductInMenuStatus.DELETED_PRODUCT_IN_MENU;
                    _unitOfWork.ProductInMenus.Update(productInMenu);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductInMenuService.DeleteProductInMenuById()]: " + e.Message);
                throw;
            }
        }


        /// <summary>
        /// Get Products In Menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuId"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<object> GetProductsInMenu(string id, string menuId, int? limit, int? page, string sort, string include)
        {
            PagingModel<ProductInMenu> pims;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            include = !string.IsNullOrEmpty(include) ? _utilService.UpperCaseFirstLetter(include) : null;

            try
            {
                pims = await _unitOfWork.ProductInMenus.GetProductInMenu(id, menuId, limit, page, isAsc, propertyName, include);
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetMenu()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendProductInMenuResponse>
            {
                List = _mapper.Map<List<ExtendProductInMenuResponse>>(pims.List),
                Page = pims.Page,
                LastPage = pims.LastPage,
                Total = pims.Total,
            };
        }


        /// <summary>
        /// Update Products In Menu By Id
        /// </summary>
        /// <param name="productInMenuUpdateRequests"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<List<ExtendProductInMenuResponse>> UpdateProductsInMenu(
            ListProductInMenuUpdateRequest productInMenuUpdateRequests)
        {
            //get list Product In Menu Id
            List<string> productInMenuIds = productInMenuUpdateRequests.ProductInMenus
                                                .Select(pim => pim.ProductInMenuId).ToList();

            //get list Product In Menu from Databases
            List<ProductInMenu> productInMenus;
            try
            {
                productInMenus = await _unitOfWork.ProductInMenus.FindListAsync(p => productInMenuIds.Contains(p.ProductInMenuId));
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.UpdateProductInMenuById()]: " + e.Message);
                throw new EntityNotFoundException();
            }

            //Update Products In Menu
            try
            {
                foreach (var productInMenu in productInMenus)
                {
                    foreach (var pimUpdate in productInMenuUpdateRequests.ProductInMenus)
                    {
                        if (productInMenu.ProductInMenuId.Equals(pimUpdate.ProductInMenuId))
                        {
                            productInMenu.Status = pimUpdate.Status;
                            productInMenu.Price = pimUpdate.Price;
                            productInMenu.UpdatedDate = _utilService.CurrentTimeInVietnam();

                            _unitOfWork.ProductInMenus.Update(productInMenu);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductInMenuService.UpdateProductInMenuById()]: " + e.Message);
                throw;
            }

            return _mapper.Map<List<ExtendProductInMenuResponse>>(productInMenus);
        }
    }
}
