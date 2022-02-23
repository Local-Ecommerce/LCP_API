using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.ProductCombination;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductCombinationService : IProductCombinationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PDC_";

        public ProductCombinationService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }


        /// <summary>
        /// Create Product Combination
        /// </summary>
        /// <param name="productCombinationRequest"></param>
        /// <returns></returns>
        public async Task<ProductCombinationResponse> CreateProductCombination(ProductCombinationRequest productCombinationRequest)
        {

            ProductCombination productCombination = _mapper.Map<ProductCombination>(productCombinationRequest);

            try
            {
                productCombination.ProductCombinationId = _utilService.CreateId(PREFIX);
                productCombination.Status = (int)ProductCombinationStatus.ACTIVE_PRODUCT_COMBINATION;

                _unitOfWork.ProductCombinations.Add(productCombination);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.CreateProductCombination()]: " + e.Message);

                throw;
            }

            return _mapper.Map<ProductCombinationResponse>(productCombination);
        }


        /// <summary>
        /// Delete Product Combination By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductCombinationResponse> DeleteProductCombinationById(string id)
        {
            //Check id
            ProductCombination productCombination;
            try
            {
                productCombination = await _unitOfWork.ProductCombinations.FindAsync(pdc => pdc.ProductCombinationId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.DeleteProductCombinationById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductCombination), id);
            }

            //Delete Product Combination
            try
            {
                productCombination.Status = (int)ProductCombinationStatus.INACTIVE_PRODUCT_COMBINATION;

                _unitOfWork.ProductCombinations.Update(productCombination);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.DeleteProductCombinationById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<ProductCombinationResponse>(productCombination);
        }


        /// <summary>
        /// Update Product Combination By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productCombinationRequest"></param>
        /// <returns></returns>
        public async Task<ProductCombinationResponse> UpdateProductCombinationById(string id, ProductCombinationRequest productCombinationRequest)
        {
            ProductCombination productCombination;
            try
            {
                productCombination = await _unitOfWork.ProductCombinations.FindAsync(m => m.ProductCombinationId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.UpdateProductCombinationById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductCombination), id);

            }

            //Update ProductCombination to DB
            try
            {
                productCombination = _mapper.Map(productCombinationRequest, productCombination);

                _unitOfWork.ProductCombinations.Update(productCombination);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.UpdateProductCombinationById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<ProductCombinationResponse>(productCombination);
        }


        /// <summary>
        /// Get Product Combination
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns> 
        public async Task<object> GetProductCombination(
            string id, string productId,
            int?[] status, int? limit,
            int? page, string sort)
        {
            PagingModel<ProductCombination> products;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                products = await _unitOfWork.ProductCombinations
                .GetProductCombination(id, productId, status, limit, page, isAsc, propertyName);

                if (_utilService.IsNullOrEmpty(products.List))
                    throw new EntityNotFoundException(typeof(ProductCombination), "in the url");
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombination.GetProductCombination()]" + e.Message);
                throw;
            }

            return new PagingModel<ProductCombinationResponse>
            {
                List = _mapper.Map<List<ProductCombinationResponse>>(products.List),
                Page = products.Page,
                LastPage = products.LastPage,
                Total = products.Total,
            };
        }
    }
}
