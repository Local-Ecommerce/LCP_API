using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.ProductCategory;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BLL.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PC_";

        public ProductCategoryService(IUnitOfWork unitOfWork,
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
        /// Create Product Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ProductCategoryResponse> CreateProCategory(ProductCategoryRequest request)
        {
            //biz rule

            //store productCategory to database
            ProductCategory productCategory = _mapper.Map<ProductCategory>(request);
            try
            {
                productCategory.ProductCategoryId = _utilService.CreateId(PREFIX);
                productCategory.Status = (int)ProductCategoryStatus.UNVERIFIED_PRODUCT_CATEGORY;
                productCategory.CreatedDate = DateTime.Now;
                productCategory.UpdatedDate = DateTime.Now;

                _unitOfWork.ProductCategories.Add(productCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.CreateProductCategory()]: " + e.Message);

                throw;
            }

            return _mapper.Map<ProductCategoryResponse>(productCategory);
        }


        /// <summary>
        /// Delete System Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductCategoryResponse> DeleteProCategory(string id)
        {
            //biz rule

            //validate id
            ProductCategory productCategory;
            try
            {
                productCategory = await _unitOfWork.ProductCategories.FindAsync(p => p.ProductCategoryId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.DeleteProductCategory()]" + e.Message);

                throw new EntityNotFoundException(typeof(ProductCategory), id);
            }

            //delete productCategory
            try
            {
                productCategory.Status = (int)ProductCategoryStatus.DELETED_PRODUCT_CATEGORY;
                _unitOfWork.ProductCategories.Update(productCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.DeleteProductCategory()]" + e.Message);

                throw;
            }

            return _mapper.Map<ProductCategoryResponse>(productCategory);
        }


        /// <summary>
        /// Update Product Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ProductCategoryResponse> UpdateProCategory(string id, ProductCategoryRequest request)
        {
            //biz rule

            //validate id
            ProductCategory productCategory;
            try
            {
                productCategory = await _unitOfWork.ProductCategories.FindAsync(p => p.ProductCategoryId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.UpdateProductCategory()]" + e.Message);

                throw new EntityNotFoundException(typeof(ProductCategory), id);
            }

            //update data
            try
            {
                productCategory = _mapper.Map(request, productCategory);
                productCategory.Status = (int)ProductCategoryStatus.UNVERIFIED_PRODUCT_CATEGORY;
                productCategory.UpdatedDate = DateTime.Now;

                _unitOfWork.ProductCategories.Update(productCategory);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.UpdateProductCategory()]" + e.Message);

                throw;
            }

            return _mapper.Map<ProductCategoryResponse>(productCategory);
        }


        /// <summary>
        /// Get Product Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<object> GetProCategory(string id, int?[] status, int? limit, int? page, string sort)
        {
            PagingModel<ProductCategory> productCategories;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                productCategories = await _unitOfWork.ProductCategories.GetProductCategory(id, status, limit, page, isAsc, propertyName);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.GetProductCategory()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendProductCategoryResponse>
            {
                List = _mapper.Map<List<ExtendProductCategoryResponse>>(productCategories.List),
                Page = productCategories.Page,
                LastPage = productCategories.LastPage,
                Total = productCategories.Total,
            };
        }


        /// <summary>
        /// Create Product Category
        /// </summary>
        /// <param name="product"></param>
        /// <param name="systemCategoryIds"></param>
        /// <returns></returns>
        public Product CreateProCategory(Product product, List<string> systemCategoryIds)
        {
            Collection<ProductCategory> productCategories = new();
            List<Product> products = new();
            products.Add(product);
            products.AddRange(product.InverseBelongToNavigation);

            //create product Category
            foreach (Product pro in products)
            {
                foreach (var systemCategoryId in systemCategoryIds)
                {
                    ProductCategory productCategory = new ProductCategory
                    {
                        ProductCategoryId = _utilService.CreateId(PREFIX),
                        Status = (int)ProductCategoryStatus.UNVERIFIED_PRODUCT_CATEGORY,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        ProductId = pro.ProductId,
                        SystemCategoryId = systemCategoryId
                    };

                    productCategories.Add(productCategory);
                }
            }

            product.ProductCategories = productCategories;
            return product;
        }
    }
}
