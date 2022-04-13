using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BLL.Dtos.ProductInMenu;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IFirebaseService _firebaseService;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private readonly IProductInMenuService _productInMenuService;
        private readonly ISystemCategoryService _systemCategoryService;
        private const string PREFIX = "PD_";
        private const string TYPE = "Product";
        private const string CACHE_KEY = "Product";
        private const string CACHE_KEY_FOR_UPDATE = "Product Before Update";


        public ProductService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService,
            IProductInMenuService productInMenuService,
            IFirebaseService firebaseService,
            ISystemCategoryService systemCategoryService)
        {
            _unitOfWork = unitOfWork;
            _firebaseService = firebaseService;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
            _productInMenuService = productInMenuService;
            _systemCategoryService = systemCategoryService;
        }


        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="baseProductRequest"></param>
        /// <returns></returns>
        public async Task<BaseProductResponse> CreateProduct(string residentId, BaseProductRequest baseProductRequest)
        {
            BaseProductResponse response;
            List<ProductInMenuRequest> pimRequest = new();

            try
            {
                //check image
                if (baseProductRequest.Image is null | baseProductRequest.Image.Count() == 0)
                    throw new BusinessException("Sản phẩm này cần hình ảnh");
                Product product = _mapper.Map<Product>(baseProductRequest);

                product.ProductId = _utilService.CreateId(PREFIX); ;
                product.Image = _firebaseService
                                        .UploadFilesToFirebase(baseProductRequest.Image, TYPE, product.ProductId, "Image", 0)
                                        .Result;
                product.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;
                product.CreatedDate = _utilService.CurrentTimeInVietnam();
                product.UpdatedDate = _utilService.CurrentTimeInVietnam();
                product.IsFavorite = 0;
                product.ApproveBy = "";
                product.BelongTo = null;
                product.ResidentId = residentId;
                product.InverseBelongToNavigation = new Collection<Product>();

                pimRequest.Add(new ProductInMenuRequest { ProductId = product.ProductId, Price = product.DefaultPrice });

                //create related product
                foreach (ProductRequest relatedProductRequest in baseProductRequest.RelatedProducts)
                {
                    Product relatedProduct = _mapper.Map<Product>(relatedProductRequest);

                    relatedProduct.ProductId = _utilService.CreateId(PREFIX);
                    relatedProduct.Image = "";
                    relatedProduct.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;
                    relatedProduct.CreatedDate = _utilService.CurrentTimeInVietnam();
                    relatedProduct.UpdatedDate = _utilService.CurrentTimeInVietnam();
                    relatedProduct.ApproveBy = "";
                    relatedProduct.IsFavorite = 0;
                    relatedProduct.ResidentId = residentId;
                    relatedProduct.BelongTo = product.ProductId;

                    product.InverseBelongToNavigation.Add(relatedProduct);

                    pimRequest.Add(new ProductInMenuRequest
                    { ProductId = relatedProduct.ProductId, Price = relatedProduct.DefaultPrice });

                }

                _unitOfWork.Products.Add(product);

                //get base menu Id
                string baseMenu = await _unitOfWork.Menus.GetBaseMenuId(residentId);

                if (baseProductRequest.ToBaseMenu)
                    //store product into base menu
                    await _productInMenuService.AddProductsToMenu(baseMenu, pimRequest);
                else
                    await _unitOfWork.SaveChangesAsync();

                response = _mapper.Map<BaseProductResponse>(product);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateBaseProduct()]: " + e.Message);
                throw;
            }

            return response;
        }


        /// <summary>
        /// Add Related Product
        /// </summary>
        /// <param name="baseProductId"></param>
        /// <param name="residentId"></param>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        public async Task AddRelatedProduct(string baseProductId, string residentId,
            List<ProductRequest> productRequests)
        {
            List<ProductInMenuRequest> pimRequest = new();
            try
            {
                //get base product
                Product baseProduct = await _unitOfWork.Products.FindAsync(p => p.ProductId.Equals(baseProductId));

                productRequests.ForEach(productRequest =>
                {
                    string productId = _utilService.CreateId(PREFIX);

                    Product product = _mapper.Map<Product>(productRequest);

                    product.ProductId = productId;
                    product.Image = "";
                    product.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;
                    product.CreatedDate = _utilService.CurrentTimeInVietnam();
                    product.UpdatedDate = _utilService.CurrentTimeInVietnam();
                    product.ApproveBy = "";
                    product.ResidentId = residentId;
                    product.BelongTo = baseProductId;
                    product.SystemCategoryId = baseProduct.SystemCategoryId;
                    pimRequest.Add(new ProductInMenuRequest { ProductId = product.ProductId, Price = product.DefaultPrice });

                    _unitOfWork.Products.Add(product);
                });

                baseProduct.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;
                _unitOfWork.Products.Update(baseProduct);

                //get base menu Id
                string baseMenu = await _unitOfWork.Menus.GetBaseMenuId(residentId);

                //check if base product in base menu
                if ((await _unitOfWork.ProductInMenus
                        .FindAsync(pim => pim.ProductId.Equals(baseProductId) && pim.MenuId.Equals(baseMenu))) != null)
                    //store product into base menu
                    await _productInMenuService.AddProductsToMenu(baseMenu, pimRequest);
                else
                    await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateRelatedProduct()]: " + e.Message);
                throw;
            }
        }


        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public async Task UpdateProduct(UpdateProductRequest productRequest)
        {
            try
            {
                //get Id of updated product
                List<string> productIds = productRequest.Products.Select(pr => pr.ProductId).ToList();

                //validate ids
                List<Product> products = await _unitOfWork.Products.FindListAsync(p => productIds.Contains(p.ProductId));

                foreach (var pR in productRequest.Products)
                {
                    //get product from database
                    Product product = products.Where(p => p.ProductId.Equals(pR.ProductId)).FirstOrDefault();

                    //store current product to Redis
                    ProductResponse currentProduct = _mapper.Map<ProductResponse>(product);
                    _redisService.StoreToList(CACHE_KEY_FOR_UPDATE, currentProduct,
                        new Predicate<ProductResponse>(up => up.ProductId.Equals(currentProduct.ProductId)));

                    string imageUrl = product.Image;

                    //update image
                    if (pR.Image != null && pR.Image.Count() > 0)
                    {
                        //get the order of the last photo
                        int order = !string.IsNullOrEmpty(product.Image) ? _utilService.LastImageNumber("Image", product.Image) : 0;

                        //upload new image & remove image
                        if (pR.Image.Length > 0)
                        {
                            foreach (var image in pR.Image)
                            {
                                if (image.Contains("https://firebasestorage.googleapis.com/"))
                                    imageUrl = imageUrl.Replace(image + "|", "");
                                else
                                    imageUrl += _firebaseService
                                        .UploadFilesToFirebase(new string[] { image }, TYPE, product.ProductId, "Image", order).Result;
                            }
                        }
                    }
                    pR.Image = null;
                    pR.ProductId = null;

                    product = _mapper.Map<ExtendProductRequest, Product>(pR, product);
                    product.Image = imageUrl;
                    product.ApproveBy = "";
                    product.UpdatedDate = _utilService.CurrentTimeInVietnam();
                    product.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;

                    _unitOfWork.Products.Update(product);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateProduct()]" + e.Message);
                throw;
            }
        }


        /// <summary>
        /// Delete Product by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task DeleteProduct(List<string> ids, string residentId)
        {
            try
            {
                List<Product> products = await _unitOfWork.Products
                .FindListAsync(p => (ids.Contains(p.ProductId) || ids.Contains(p.BelongTo))
                                        && p.ResidentId.Equals(residentId));

                //delete product
                products.ForEach(product =>
                            {
                                product.Status = (int)ProductStatus.DELETED_PRODUCT;
                                product.UpdatedDate = _utilService.CurrentTimeInVietnam();
                                product.ApproveBy = "";

                                _unitOfWork.Products.Update(product);
                            });

                //delete products in menu
                List<ProductInMenu> productInMenus = await _unitOfWork.ProductInMenus.FindListAsync(p => ids.Contains(p.ProductId));
                foreach (var productInMenu in productInMenus)
                {
                    productInMenu.Status = (int)ProductInMenuStatus.DELETED_PRODUCT_IN_MENU;
                    _unitOfWork.ProductInMenus.Update(productInMenu);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteBaseProduct()]" + e.Message);
                throw;
            }

        }


        /// <summary>
        /// Verify Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<BaseProductResponse> VerifyProductById(string productId, bool isApprove, string residentId)
        {
            BaseProductResponse productResponse;

            try
            {
                //get product from database
                Product baseProduct =
                    (await _unitOfWork.Products
                        .GetProduct(id: productId, include: new string[] { "related" }))
                        .List
                        .First();

                //verify product
                baseProduct.UpdatedDate = _utilService.CurrentTimeInVietnam();
                baseProduct.Status = isApprove ? (int)ProductStatus.VERIFIED_PRODUCT : (int)ProductStatus.REJECTED_PRODUCT;
                baseProduct.ApproveBy = isApprove ? residentId : "";


                //verify related product
                for (int i = 0; i < baseProduct.InverseBelongToNavigation.Count; i++)
                {
                    Product relatedProduct = baseProduct.InverseBelongToNavigation.ElementAt(i);
                    baseProduct.InverseBelongToNavigation.Remove(relatedProduct);

                    relatedProduct.UpdatedDate = _utilService.CurrentTimeInVietnam();
                    relatedProduct.Status = isApprove ? (int)ProductStatus.VERIFIED_PRODUCT : (int)ProductStatus.REJECTED_PRODUCT;
                    relatedProduct.ApproveBy = isApprove ? residentId : "";

                    baseProduct.InverseBelongToNavigation.Add(relatedProduct);
                }

                _unitOfWork.Products.Update(baseProduct);

                await _unitOfWork.SaveChangesAsync();

                //remove from redis
                _redisService.DeleteFromList(CACHE_KEY_FOR_UPDATE,
                    new Predicate<ProductResponse>(p => p.ProductId.Equals(baseProduct.ProductId)));

                productResponse = _mapper.Map<BaseProductResponse>(baseProduct);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.VerifyCreateProductById()]: " + e.Message);
                throw;
            }

            return productResponse;
        }


        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="apartmentId"></param>
        /// <param name="sysCateId"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <param name="residentId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<PagingModel<BaseProductResponse>> GetProduct(
            string id = default, int?[] status = default, string apartmentId = default,
            string sysCateId = default, string search = default, int? limit = default, int? page = default,
            string sort = default, string[] include = default, string residentId = default, string role = default)
        {
            // get product for market manager
            if (role.Equals(ResidentType.MARKET_MANAGER))
            {
                if (id != null && status.Contains((int)ProductStatus.UNVERIFIED_PRODUCT))
                    return await GetUnverifiedProductForMarketManager(id);
                else if (!status.Contains((int)ProductStatus.UNVERIFIED_PRODUCT))
                    return await GetProductForCustomer(id, residentId, sysCateId, search);
            }

            PagingModel<Product> products;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            if (role != null && !role.Equals(ResidentType.MERCHANT))
                residentId = null;

            try
            {
                products = await _unitOfWork.Products.GetProduct
                    (id, status, apartmentId, sysCateId, search, limit, page, isAsc, propertyName, include, residentId);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetProduct()]" + e.Message);
                throw;
            }

            List<BaseProductResponse> responses = _mapper.Map<List<BaseProductResponse>>(products.List);

            return new PagingModel<BaseProductResponse>
            {
                List = responses,
                Page = products.Page,
                LastPage = products.LastPage,
                Total = products.Total,
            };
        }


        /// <summary>
        /// Get Unverified Product For MarketManager
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PagingModel<BaseProductResponse>> GetUnverifiedProductForMarketManager(string id)
        {
            List<BaseProductResponse> responses = new();
            try
            {
                List<UpdateProductResponse> products = _mapper.Map<List<UpdateProductResponse>>(
                            await _unitOfWork.Products.GetProductsById(id));

                List<string> productIds = products.Select(p => p.ProductId)
                                                    .ToList();
                List<ProductResponse> currentProducts = _redisService.GetList<ProductResponse>(CACHE_KEY_FOR_UPDATE)
                    .FindAll(p => productIds.Contains(p.ProductId));

                foreach (var product in products)
                    product.CurrentProduct = currentProducts.Find(p => p.ProductId.Equals(product.ProductId));

                BaseProductResponse response = _mapper.Map<BaseProductResponse>(products.Find(p => p.BelongTo == null));
                products.Remove(products.Find(p => p.BelongTo == null));
                response.RelatedProducts = new Collection<UpdateProductResponse>(products);

                responses.Add(response);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetUnverifiedProductForMarketManager()]" + e.Message);
                throw;
            }

            return new PagingModel<BaseProductResponse>
            {
                List = responses,
                Page = 1,
                LastPage = 1,
                Total = 1
            };
        }


        /// <summary>
        /// Get Product For Customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentId"></param>
        /// <param name="sysCateId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PagingModel<BaseProductResponse>> GetProductForCustomer(
            string id, string residentId, string sysCateId, string search)
        {
            List<BaseProductResponse> responses = new();
            List<UpdateProductResponse> allProducts = new();
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vnTime = TimeZoneInfo.ConvertTime(_utilService.CurrentTimeInVietnam(), vnZone);
            try
            {
                //get all category belong to syscateId
                List<string> categoryIds = await _systemCategoryService.GetSystemCategoryIdsById(sysCateId);
                //get apartment of resident
                string apartmentId = (await _unitOfWork.Residents.FindAsync(r => r.ResidentId.Equals(residentId))).ApartmentId;

                //get active menu
                List<Menu> menus = (await _unitOfWork.Menus
                        .GetMenu(null, new int?[] { (int)MenuStatus.ACTIVE_MENU }, null, apartmentId, null, null, true,
                            null, null, null, null, new string[] { "product" })).List;

                //base menus
                List<Menu> baseMenus = menus.Where(menu => (bool)menu.BaseMenu).ToList();
                //other menus : menus
                menus.RemoveAll(menu => (bool)menu.BaseMenu);

                //add products from other menus
                if (!_utilService.IsNullOrEmpty(menus))
                    foreach (Menu menu in menus)
                    {
                        allProducts.AddRange(GetProductFromMenuBySysCateIdAndProductId(id, categoryIds, menu, allProducts));

                        //check if menu includes base menu
                        if ((bool)menu.IncludeBaseMenu)
                        {
                            //get base menu
                            Menu baseMenu = baseMenus.Where(mn => mn.MerchantStoreId.Equals(menu.MerchantStoreId)).FirstOrDefault();
                            if (baseMenu != null)
                            {
                                baseMenus.Remove(baseMenu);

                                //add product from this base menu
                                allProducts.AddRange(GetProductFromMenuBySysCateIdAndProductId(id, categoryIds, baseMenu, allProducts));
                            }
                        }
                    }

                //add products from the remaining base menus
                if (!_utilService.IsNullOrEmpty(baseMenus))
                    foreach (Menu menu in baseMenus)
                        allProducts.AddRange(GetProductFromMenuBySysCateIdAndProductId(id, categoryIds, menu, allProducts));

                //create response
                foreach (var product in allProducts)
                {
                    if (product.BelongTo == null)
                    {
                        BaseProductResponse response = _mapper.Map<BaseProductResponse>(product);
                        List<UpdateProductResponse> rP = allProducts.Where(p => p.BelongTo != null && p.BelongTo.Equals(product.ProductId)).ToList();
                        response.RelatedProducts = new Collection<UpdateProductResponse>(rP);

                        responses.Add(response);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetProductForCustomer()]: " + e.Message);
                throw;
            }
            return new PagingModel<BaseProductResponse>
            {
                List = responses,
                Page = 1,
                LastPage = 1,
                Total = responses.Count,
            };
        }


        /// <summary>
        /// Get Product From Menu By SysCateId And ProductId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sysCateIds"></param>
        /// <param name="menu"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public List<UpdateProductResponse> GetProductFromMenuBySysCateIdAndProductId(
            string productId, List<string> sysCateIds, Menu menu,
            List<UpdateProductResponse> products)
        {
            List<UpdateProductResponse> responses = new();
            List<ProductInMenu> pims = !_utilService.IsNullOrEmpty(sysCateIds) ? menu.ProductInMenus
                            .Where(pim => sysCateIds.Contains(pim.Product.SystemCategoryId))
                            .ToList() : menu.ProductInMenus.ToList();

            if (!_utilService.IsNullOrEmpty(pims))
            {
                foreach (var pim in pims)
                {
                    UpdateProductResponse response = _mapper.Map<UpdateProductResponse>(pim.Product);
                    if (productId != null)
                    {
                        if (!(response.ProductId.Equals(productId) ||
                                (response.BelongTo != null && response.BelongTo.Equals(productId))))
                            continue;
                    }

                    //check if it was already in list
                    UpdateProductResponse product = products.Where(p => p.ProductId.Equals(response.ProductId)).FirstOrDefault();
                    if (product == null)
                    {
                        response.DefaultPrice = pim.Price;
                        responses.Add(response);
                    }
                }
            }
            return responses;
        }


        /// <summary>
        /// Get Product Price For Order
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInfoForOrder> GetProductPriceForOrder(string productId)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vnTime = TimeZoneInfo.ConvertTime(_utilService.CurrentTimeInVietnam(), vnZone);

            try
            {
                Product product = (await _unitOfWork.Products.GetProduct(id: productId, include: new string[] { "menu" }))
                                    .List
                                    .First();

                //get price not in base menu
                List<ProductInMenu> pdNotInBaseMenu = product.ProductInMenus.Where(pim => !(bool)pim.Menu.BaseMenu).ToList();
                if (!_utilService.IsNullOrEmpty(pdNotInBaseMenu))
                    foreach (ProductInMenu pim in pdNotInBaseMenu)
                    {
                        //get active menu
                        if (TimeSpan.Compare(vnTime.TimeOfDay, (TimeSpan)pim.Menu.TimeStart) > 0 &&
                                TimeSpan.Compare(vnTime.TimeOfDay, (TimeSpan)pim.Menu.TimeEnd) < 0 &&
                                pim.Menu.Status.Equals((int)MenuStatus.ACTIVE_MENU))
                        {
                            return new ProductInfoForOrder
                            {
                                MerchantStoreId = pim.Menu.MerchantStoreId,
                                Price = (double)pim.Price,
                                ProductInMenuId = pim.ProductInMenuId
                            };
                        }
                    }

                //get base menu
                ProductInMenu pdInBaseMenu = product.ProductInMenus.Where(pim => (bool)pim.Menu.BaseMenu).FirstOrDefault();
                if (pdInBaseMenu != null)
                {
                    return new ProductInfoForOrder
                    {
                        MerchantStoreId = pdInBaseMenu.Menu.MerchantStoreId,
                        Price = (double)pdInBaseMenu.Price,
                        ProductInMenuId = pdInBaseMenu.ProductInMenuId
                    };
                }
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetProductPriceForOrder()]: " + e.Message);
                throw;
            }
            return null;
        }
    }
}
