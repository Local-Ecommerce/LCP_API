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
        private readonly IRedisService _redisService;
        private const string PREFIX = "PIM_";
        private const string CACHE_KEY = "Quantity";

        public ProductInMenuService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _redisService = redisService;
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
                DateTime vnTime = _utilService.CurrentTimeInVietnam();
                productInMenus.ForEach(productInMenu =>
                {
                    productInMenu.ProductInMenuId = _utilService.CreateId(PREFIX);
                    productInMenu.MenuId = menuId;
                    productInMenu.CreatedDate = vnTime;
                    productInMenu.UpdatedDate = vnTime;
                    productInMenu.Status = (int)ProductInMenuStatus.ACTIVE_PRODUCT_IN_MENU;

                    _unitOfWork.ProductInMenus.Add(productInMenu);

                    _redisService.StoreToList(CACHE_KEY,
                        new ProductQuantityDto()
                        {
                            ProductId = productInMenu.ProductId,
                            ProductInMenuId = productInMenu.ProductInMenuId,
                            Quantity = productInMenu.Quantity.Value,
                            UpdatedDate = vnTime
                        },
                        new Predicate<ProductQuantityDto>(p => p.ProductId.Equals(productInMenu.ProductId) && p.ProductInMenuId.Equals(productInMenu.ProductInMenuId)));
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
                productInMenus = await _unitOfWork.ProductInMenus.GetProductsInMenu(productInMenuIds);
                foreach (var productInMenu in productInMenus)
                {
                    if (!_utilService.IsNullOrEmpty(productInMenu.Product.InverseBelongToNavigation))
                    {
                        foreach (var product in productInMenu.Product.InverseBelongToNavigation)
                        {
                            if (!productInMenuIds.Contains(product.ProductId))
                            {
                                ProductInMenu pim = await _unitOfWork.ProductInMenus
                                    .FindAsync(pim => pim.ProductId.Equals(product.ProductId));
                                pim.Status = (int)ProductInMenuStatus.DELETED_PRODUCT_IN_MENU;
                                _unitOfWork.ProductInMenus.Update(pim);
                            }
                        }
                    }

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
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<object> GetProductsInMenu(string id, string menuId, int?[] status,
            int? limit, int? page, string sort, string include)
        {
            string propertyName = default;
            bool isAsc = false;

            List<BaseProductInMenuResponse> responses = new();

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            include = !string.IsNullOrEmpty(include) ? _utilService.UpperCaseFirstLetter(include) : null;

            try
            {
                List<ProductInMenu> pims =
                    (await _unitOfWork.ProductInMenus.GetProductInMenu(id, menuId, status, limit, page, isAsc, propertyName, include))
                    .List;

                //check if contains product
                List<ProductInMenu> pimsContainProduct = pims.Where(pim => pim.Product != null).ToList();
                if (!_utilService.IsNullOrEmpty(pimsContainProduct))
                {
                    List<ExtendProductInMenuResponse> extendPIMResponses =
                        _mapper.Map<List<ExtendProductInMenuResponse>>(pimsContainProduct);

                    //get all Product' Id
                    List<string> productIds = extendPIMResponses.Select(pim => pim.ProductId).ToList();

                    foreach (var extendPIMResponse in extendPIMResponses)
                    {
                        if (extendPIMResponse.Product.BelongTo == null)
                        {
                            BaseProductInMenuResponse basePIM = _mapper.Map<BaseProductInMenuResponse>(extendPIMResponse);
                            basePIM.RelatedProductInMenu = extendPIMResponses
                                .FindAll(pim => pim.Product.BelongTo != null && pim.Product.BelongTo.Equals(basePIM.ProductId));

                            //remove id from all Product' Id
                            productIds.Remove(basePIM.ProductId);
                            List<string> relatedPIMId = basePIM.RelatedProductInMenu.Select(pim => pim.ProductId).ToList();
                            productIds = productIds.Except(relatedPIMId).ToList();

                            responses.Add(basePIM);
                        }
                    }

                    //check if any product are missing
                    if (!_utilService.IsNullOrEmpty(productIds))
                    {
                        foreach (var productId in productIds)
                        {
                            ExtendProductInMenuResponse pim = extendPIMResponses
                                .Where(pim => pim.ProductId.Equals(productId))
                                .FirstOrDefault();
                            if (pim != null) responses.Add(_mapper.Map<BaseProductInMenuResponse>(pim));
                        }
                    }
                }

                //if not contains products
                if (_utilService.IsNullOrEmpty(responses))
                    responses = _mapper.Map<List<BaseProductInMenuResponse>>(pims);
            }
            catch (Exception e)
            {
                _logger.Error("[MenuService.GetMenu()]" + e.Message);
                throw;
            }

            return responses;
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
            DateTime vnTime = _utilService.CurrentTimeInVietnam();

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
                            productInMenu.UpdatedDate = vnTime;
                            productInMenu.Quantity = pimUpdate.Quantity;
                            productInMenu.MaxBuy = pimUpdate.MaxBuy;

                            _unitOfWork.ProductInMenus.Update(productInMenu);

                            //update quantity in Redis
                            _redisService.StoreToList(CACHE_KEY,
                            new ProductQuantityDto()
                            {
                                ProductId = productInMenu.ProductId,
                                Quantity = productInMenu.Quantity.Value,
                                ProductInMenuId = productInMenu.ProductInMenuId,
                                UpdatedDate = vnTime
                            },
                            new Predicate<ProductQuantityDto>(pqd => pqd.ProductId.Equals(productInMenu.ProductId) && pqd.ProductInMenuId.Equals(productInMenu.ProductInMenuId)));
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


        /// <summary>
        /// Get Product In Menu For Order
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInMenu> GetProductInMenuForOrder(string productId)
        {
            DateTime vnTime = _utilService.CurrentTimeInVietnam();
            try
            {
                Product product = await _unitOfWork.Products.GetProductIncludeStore(productId);

                //get pim not in base menu
                List<ProductInMenu> pdNotInBaseMenu = product.ProductInMenus.Where(pim => !(bool)pim.Menu.BaseMenu).ToList();
                if (!_utilService.IsNullOrEmpty(pdNotInBaseMenu))
                    foreach (ProductInMenu pim in pdNotInBaseMenu)
                    {
                        //get active menu
                        if (TimeSpan.Compare(vnTime.TimeOfDay, (TimeSpan)pim.Menu.TimeStart) > 0 &&
                                TimeSpan.Compare(vnTime.TimeOfDay, (TimeSpan)pim.Menu.TimeEnd) < 0 &&
                                pim.Status.Equals((int)ProductInMenuStatus.ACTIVE_PRODUCT_IN_MENU) &&
                                pim.Menu.Status.Equals((int)MenuStatus.ACTIVE_MENU) &&
                                pim.Menu.RepeatDate.Contains($"{(int)vnTime.DayOfWeek}") &&
                                pim.Quantity.Value > 0)

                            return pim;
                    }

                //get base menu
                ProductInMenu pdInBaseMenu = product.ProductInMenus.Where(pim => (bool)pim.Menu.BaseMenu).FirstOrDefault();
                if (pdInBaseMenu != null)
                    return pdInBaseMenu;
            }
            catch (Exception e)
            {
                _logger.Error("[ProductInMenuService.GetProductInMenuForOrder()]: " + e.Message);
                throw;
            }

            return null;
        }
    }
}
