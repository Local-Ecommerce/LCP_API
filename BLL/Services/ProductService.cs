using AutoMapper;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<BaseResponse<ProductResponse>> CreateProduct(ProductRequest productRequest)
        {
            //biz rule

            //upload image
            string imageUrl = productRequest.Image.ToString();

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
                product.UpdatedBy = "";

                _unitOfWork.Repository<Product>().Add(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateProduct()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ProductResponse>
                    {
                        ResultCode = (int)ProductStatus.ERROR,
                        ResultMessage = ProductStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            //store product to Redis
            _redisService.StoreToList(CACHE_KEY, productResponse,
                    new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));

            return new BaseResponse<ProductResponse>
            {
                ResultCode = (int)ProductStatus.SUCCESS,
                ResultMessage = ProductStatus.SUCCESS.ToString(),
                Data = productResponse
            };
        }


        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ProductResponse>> GetProductById(string id)
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
                    Product product = await _unitOfWork.Repository<Product>()
                                                       .FindAsync(p => p.ProductId.Equals(id));
                    productResponse = _mapper.Map<ProductResponse>(product);

                }
                catch (Exception e)
                {
                    _logger.Error("[ProductService.GetProductById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<ProductResponse>
                        {
                            ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                            ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<ProductResponse>
            {
                ResultCode = (int)ProductStatus.SUCCESS,
                ResultMessage = ProductStatus.SUCCESS.ToString(),
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
        public async Task<BaseResponse<ProductResponse>> UpdateProduct(string id,
            ProductRequest productRequest)
        {
            //biz rule

            //validate id
            Product product;
            try
            {
                product = await _unitOfWork.Repository<Product>()
                                           .FindAsync(p => p.ProductId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                        ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //upload image
            string imageUrl = productRequest.Image.ToString();

            //update data
            try
            {
                product = _mapper.Map(productRequest, product);
                product.Image = imageUrl;
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = "";

                _unitOfWork.Repository<Product>().Update(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)ProductStatus.ERROR,
                        ResultMessage = ProductStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            //store product to Redis
            _redisService.StoreToList(CACHE_KEY, productResponse,
                    new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));

            return new BaseResponse<ProductResponse>
            {
                ResultCode = (int)ProductStatus.SUCCESS,
                ResultMessage = ProductStatus.SUCCESS.ToString(),
                Data = productResponse
            };
        }


        /// <summary>
        /// DeleteProduct by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ProductResponse>> DeleteProduct(string id)
        {
            //biz rule

            //validate id
            Product product;
            try
            {
                product = await _unitOfWork.Repository<Product>()
                                           .FindAsync(p => p.ProductId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                        ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //delete product
            try
            {
                product.IsDeleted = true;
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = "";

                _unitOfWork.Repository<Product>().Update(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)ProductStatus.ERROR,
                        ResultMessage = ProductStatus.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            //store product to Redis
            _redisService.StoreToList(CACHE_KEY, productResponse,
                new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));

            return new BaseResponse<ProductResponse>
            {
                ResultCode = (int)ProductStatus.SUCCESS,
                ResultMessage = ProductStatus.SUCCESS.ToString(),
                Data = productResponse
            };

        }
    }
}
