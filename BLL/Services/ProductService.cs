using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "Product";

        public ProductService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
        }


        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <param name="image">list of product's image</param>
        /// <returns></returns>
        public BaseResponse<ProductResponse> CreateProduct(ProductRequest productRequest,
            List<IFormFile> image)
        {
            //biz rule

            //upload image
            string imageUrl = "";

            //store product to database
            Product product = _mapper.Map<Product>(productRequest);
            try
            {
                product.ProductId = _utilService.Create16Alphanumeric();
                product.Image = imageUrl;
                product.IsActive = false;
                product.IsDeleted = false;
                product.CreatedDate = DateTime.Now;
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = "Hân đẹp trai";

                _unitOfWork.Repository<Product>().Add(product);

                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateProduct()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ProductResponse>
                    {
                        ResultCode = ResultCode.ERROR_CODE,
                        ResultMessage = ResultCode.ERROR_MESSAGE,
                        Data = default
                    });
            }

            //create response
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            //store product to Redis
            StoreProductToRedis(productResponse);

            return new BaseResponse<ProductResponse>
            {
                ResultCode = ResultCode.SUCCESS_CODE,
                ResultMessage = ResultCode.SUCCESS_MESSAGE,
                Data = productResponse
            };
        }


        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<ProductResponse> GetProductById(string id)
        {
            ProductResponse productResponse = null;

            //get product from Redis
            productResponse = _redisService.GetList<ProductResponse>(CACHE_KEY)
                .Find(product => product.ProductId.Equals(id));

            if (productResponse is null)
            {
                //get product from database
                try
                {
                    Product product = _unitOfWork.Repository<Product>().Get(id);
                    productResponse = _mapper.Map<ProductResponse>(product);
                }
                catch (Exception e)
                {
                    _logger.Error("[ProductService.GetProductById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<ProductResponse>
                        {
                            ResultCode = ResultCode.PRODUCT_NOT_FOUND_CODE,
                            ResultMessage = ResultCode.PRODUCT_NOT_FOUND_MESSAGE,
                            Data = default
                        });
                }
            }

            return new BaseResponse<ProductResponse>
            {
                ResultCode = ResultCode.SUCCESS_CODE,
                ResultMessage = ResultCode.SUCCESS_MESSAGE,
                Data = productResponse
            };
        }


        /// <summary>
        /// Update a product by Id
        /// </summary>
        /// <param name="id">id of product</param>
        /// <param name="productRequest"></param>
        /// <param name="image">list of product's image</param>
        /// <returns></returns>
        public BaseResponse<ProductResponse> UpdateProduct(string id,
            ProductRequest productRequest,
            List<IFormFile> image)
        {
            //biz rule

            //validate id
            Product product;
            try
            {
                product = _unitOfWork.Repository<Product>().Get(id);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = ResultCode.PRODUCT_NOT_FOUND_CODE,
                        ResultMessage = ResultCode.PRODUCT_NOT_FOUND_MESSAGE,
                        Data = default
                    });
            }

            //upload image
            string imageUrl = image.ToString();

            //update data
            try
            {
                product = _mapper.Map(productRequest, product);
                product.Image = imageUrl;
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = "Hân đẹp trai";

                _unitOfWork.Repository<Product>().Update(product);

                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = ResultCode.ERROR_CODE,
                        ResultMessage = ResultCode.ERROR_MESSAGE,
                        Data = default
                    });
            }

            //create response
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            //store product to Redis
            StoreProductToRedis(productResponse);

            return new BaseResponse<ProductResponse>
            {
                ResultCode = ResultCode.SUCCESS_CODE,
                ResultMessage = ResultCode.SUCCESS_MESSAGE,
                Data = productResponse
            };
        }


        /// <summary>
        /// DeleteProduct by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<ProductResponse> DeleteProduct(string id)
        {
            //biz rule

            //validate id
            Product product;
            try
            {
                product = _unitOfWork.Repository<Product>().Get(id);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = ResultCode.PRODUCT_NOT_FOUND_CODE,
                        ResultMessage = ResultCode.PRODUCT_NOT_FOUND_MESSAGE,
                        Data = default
                    });
            }

            //delete product
            try
            {
                product.IsDeleted = true;
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = "Hân đẹp trai";

                _unitOfWork.Repository<Product>().Update(product);

                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = ResultCode.ERROR_CODE,
                        ResultMessage = ResultCode.ERROR_MESSAGE,
                        Data = default
                    });
            }

            //create response
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            //store product to Redis
            StoreProductToRedis(productResponse);

            return new BaseResponse<ProductResponse>
            {
                ResultCode = ResultCode.SUCCESS_CODE,
                ResultMessage = ResultCode.SUCCESS_MESSAGE,
                Data = productResponse
            };

        }


        /// <summary>
        /// Store product to Redis
        /// </summary>
        /// <param name="product"></param>
        public void StoreProductToRedis(ProductResponse product)
        {
            List<ProductResponse> products = _redisService.GetList<ProductResponse>(CACHE_KEY);

            //check list of products is null or empty
            if (_utilService.IsNullOrEmpty(products))
            {
                products = new List<ProductResponse>();
            }

            //check if the product exists or not
            ProductResponse p = products.Find(p => p.ProductId.Equals(product.ProductId));

            if (p != null)
            {
                products.Remove(p);
            }

            products.Add(product);

            _redisService.StoreList(CACHE_KEY, products);
        }
    }
}
