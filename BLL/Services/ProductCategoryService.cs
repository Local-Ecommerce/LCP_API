using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.ProductCategory;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                productCategory.Status = (int)ProductCategoryStatus.UNVERIFIED_CREATE_PRODUCT_CATEGORY;
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
                productCategory.Status = (int)ProductCategoryStatus.UNVERIFIED_UPDATE_PRODUCT_CATEGORY;
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
        /// Get Product Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendProductCategoryResponse> GetProCategoryById(string id)
        {
            ExtendProductCategoryResponse extendProductCategoryResponse;

            //get productCategory from database
            try
            {
                ProductCategory productCategory = await _unitOfWork.ProductCategories.FindAsync(p => p.ProductCategoryId.Equals(id));
                extendProductCategoryResponse = _mapper.Map<ExtendProductCategoryResponse>(productCategory);

            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.GetProductCategoryById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductCategory), id);
            }

            return extendProductCategoryResponse;
        }


        /// <summary>
        /// Get Product Categories By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<ExtendProductCategoryResponse>> GetProductCategoriesByStatus(int status)
        {
            List<ExtendProductCategoryResponse> productCategoryList = null;

            //get ProductCategory from database
            try
            {
                productCategoryList = _mapper.Map<List<ExtendProductCategoryResponse>>(
                    await _unitOfWork.ProductCategories.FindListAsync(ms => ms.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCategoryService.GetProductCategorysByStatus()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductCategory), status);
            }

            return productCategoryList;
        }
    }
}
