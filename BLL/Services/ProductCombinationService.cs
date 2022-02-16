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
        /// <exception cref="HttpStatusException"></exception>
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
        /// <exception cref="HttpStatusException"></exception>
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
        /// Get Product Combination By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<ProductCombinationResponse> GetProductCombinationById(string id)
        {
            ProductCombinationResponse productCombinationReponse = null;

            //Get ProductCombination from DB
            if (productCombinationReponse is null)
            {
                try
                {
                    ProductCombination productCombination = await _unitOfWork.ProductCombinations.FindAsync(pdc => pdc.ProductCombinationId.Equals(id));

                    productCombinationReponse = _mapper.Map<ProductCombinationResponse>(productCombination);
                }
                catch (Exception e)
                {
                    _logger.Error("[ProductCombinationService.GetProductCombinationById()]: " + e.Message);

                    throw new EntityNotFoundException(typeof(ProductCombination), id);
                }
            }

            return productCombinationReponse;
        }


        /// <summary>
        /// Get Product Combinations By Base Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<ProductCombinationResponse>> GetProductCombinationsByBaseProductId(string id)
        {
            List<ProductCombinationResponse> productCombinationList = null;

            //get ProductCombination from database
            try
            {
                productCombinationList = _mapper.Map<List<ProductCombinationResponse>>(
                    await _unitOfWork.ProductCombinations
                                     .FindListAsync(me => me.BaseProductId.Equals(id)));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.GetProductCombinationsByBaseProductId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductCombination), id);
            }

            return productCombinationList;
        }


        /// <summary>
        /// Get Product Combinations By Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<ProductCombinationResponse>> GetProductCombinationsByProductId(string id)
        {
            List<ProductCombinationResponse> productCombinationList = null;

            //get ProductCombination from database
            try
            {
                productCombinationList = _mapper.Map<List<ProductCombinationResponse>>(
                    await _unitOfWork.ProductCombinations
                                     .FindListAsync(me => me.ProductId.Equals(id)));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.GetProductCombinationsByProductId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductCombination), id);
            }

            return productCombinationList;
        }


        /// <summary>
        /// Get Product Combinations By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<ProductCombinationResponse>> GetProductCombinationsByStatus(int status)
        {
            List<ProductCombinationResponse> productCombinationList = null;

            //get ProductCombination from database
            try
            {
                productCombinationList = _mapper.Map<List<ProductCombinationResponse>>(
                    await _unitOfWork.ProductCombinations
                                     .FindListAsync(me => me.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductCombinationService.GetProductCombinationsByStatus()]: " + e.Message);

                throw new EntityNotFoundException(typeof(ProductCombination), status);
            }

            return productCombinationList;
        }


        /// <summary>
        /// Update Product Combination By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productCombinationRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
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
    }
}
