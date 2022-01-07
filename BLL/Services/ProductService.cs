using AutoMapper;
using BLL.Dtos;
using BLL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PD_";
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
        /// Create base product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BaseProductResponse>> CreateBaseProduct(ProductRequest productRequest)
        {
            //biz rule

            //upload image
            string imageUrl = productRequest.Image.ToString();

            //store product to database
            Product product = _mapper.Map<Product>(productRequest);
            try
            {
                product.ProductId = _utilService.CreateId(PREFIX);
                product.Image = imageUrl;
                product.Status = (int)ProductStatus.UNVERIFIED_CREATE_PRODUCT;
                product.CreatedDate = DateTime.Now;
                product.UpdatedDate = DateTime.Now;
                product.ApproveBy = "";
                product.BelongTo = null;

                _unitOfWork.Repository<Product>().Add(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateBaseProduct()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<BaseProductResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //store product response to Redis
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            _redisService.StoreToList(CACHE_KEY, productResponse,
                    new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));


            //create base product response
            BaseProductResponse baseProductResponse = _mapper.Map<BaseProductResponse>(product);
            baseProductResponse.RelatedProducts = new List<ProductResponse>();

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = baseProductResponse
            };
        }


        /// <summary>
        /// Create Related Product
        /// </summary>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BaseProductResponse>> CreateRelatedProduct(string baseProductId,
            List<ProductRequest> productRequests)
        {
            //biz rule

            //upload image
            //string imageUrl = productRequest.Image.ToString();

            //store related products to database
            List<Product> products = _mapper.Map<List<Product>>(productRequests);
            try
            {
                products.ForEach(product =>
                {
                    product.ProductId = _utilService.CreateId(PREFIX);
                    product.Image = "";
                    product.Status = (int)ProductStatus.UNVERIFIED_CREATE_PRODUCT;
                    product.CreatedDate = DateTime.Now;
                    product.UpdatedDate = DateTime.Now;
                    product.ApproveBy = "";
                    product.BelongTo = baseProductId;

                    _unitOfWork.Repository<Product>().Add(product);
                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateRelatedProduct()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<BaseProductResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }


            //store new related product to Redis
            List<ProductResponse> productResponses = _mapper.Map<List<ProductResponse>>(products);
            productResponses.ForEach(product =>
            {
                _redisService.StoreToList(CACHE_KEY, product,
                    new Predicate<ProductResponse>(p => p.ProductId == product.ProductId));
            });

            //create response
            BaseProductResponse baseProductResponse = GetBaseProductById(baseProductId).Result.Data;
            baseProductResponse.RelatedProducts = GetRelatedProductsByBaseProductId(baseProductId).Result.Data;

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = baseProductResponse
            };
        }


        /// <summary>
        /// Get Base Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BaseProductResponse>> GetBaseProductById(string id)
        {
            BaseProductResponse baseProductResponse = null;

            ProductResponse productResponse = null;

            //get product from Redis
            productResponse = _redisService.GetList<ProductResponse>(CACHE_KEY)
                .Find(product => product.ProductId.Equals(id));

            if (productResponse != null)
            {
                baseProductResponse = _mapper.Map<BaseProductResponse>(productResponse);

                    baseProductResponse.RelatedProducts = _redisService.GetList<ProductResponse>(CACHE_KEY)
                        .Where(p => p.BelongTo != null && p.BelongTo.Equals(id))
                        .ToList();
            }
            else
            {
                //get product from database
                try
                {

                    List<Product> products = await _unitOfWork.Repository<Product>()
                        .FindListAsync(p => p.ProductId.Equals(id) || p.BelongTo.Equals(id));

                    Product product = products.Find(p => p.ProductId.Equals(id));
                    products.Remove(product);

                    baseProductResponse = _mapper.Map<BaseProductResponse>(product);
                    baseProductResponse.RelatedProducts = _mapper.Map<List<ProductResponse>>(products);

                }
                catch (Exception e)
                {
                    _logger.Error("[ProductService.GetBaseProductById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<BaseProductResponse>
                        {
                            ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                            ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = baseProductResponse
            };
        }


        /// <summary>
        /// Get Related Products By Base Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ProductResponse>>> GetRelatedProductsByBaseProductId(string id)
        {
            List<ProductResponse> productResponses = null;

            //get product from Redis
            productResponses = _redisService.GetList<ProductResponse>(CACHE_KEY)
                .Where(p => p.BelongTo != null && p.BelongTo.Equals(id))
                .ToList();

            if (_utilService.IsNullOrEmpty(productResponses))
            {
                //get product from database
                try
                {

                    List<Product> products = await _unitOfWork.Repository<Product>()
                        .FindListAsync(p => p.BelongTo.Equals(id));

                    productResponses = _mapper.Map<List<ProductResponse>>(products);
                }
                catch (Exception e)
                {
                    _logger.Error("[ProductService.GetRelatedProductsByBaseProductId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<BaseProductResponse>
                        {
                            ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                            ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<List<ProductResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponses
            };
        }


        /// <summary>
        /// Get Related Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ProductResponse>> GetRelatedProductById(string id)
        {
            ProductResponse productResponse = null;

            //get product from Redis
            productResponse = _redisService.GetList<ProductResponse>(CACHE_KEY)
                .Find(product => product.ProductId.Equals(id));

            if (productResponse == null)
                //get product from database
                try
                {

                    Product product = await _unitOfWork.Repository<Product>()
                        .FindAsync(p => p.ProductId.Equals(id));

                    productResponse = _mapper.Map<ProductResponse>(product);
                }
                catch (Exception e)
                {
                    _logger.Error("[ProductService.GetRelatedProductById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<BaseProductResponse>
                        {
                            ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                            ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }

            return new BaseResponse<ProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponse
            };
        }


        /// <summary>
        /// Update base product by Id
        /// </summary>
        /// <param name="id">id of product</param>
        /// <param name="productRequest"></param>
        /// <param name="image">list of product's image</param>
        /// <returns></returns>
        public async Task<BaseResponse<BaseProductResponse>> UpdateBaseProduct(string id,
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
                _logger.Error("[ProductService.UpdateBaseProduct()]" + e.Message);

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
                product.Status = (int)ProductStatus.UNVERIFIED_UPDATE_PRODUCT;
                product.ApproveBy = "";

                _unitOfWork.Repository<Product>().Update(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateBaseProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }



            //store product to Redis
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
            _redisService.StoreToList(CACHE_KEY, productResponse,
                    new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));

            //create response
            BaseProductResponse baseProductResponse = _mapper.Map<BaseProductResponse>(product);
            baseProductResponse.RelatedProducts = GetRelatedProductsByBaseProductId(baseProductResponse.ProductId).Result.Data;

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = baseProductResponse
            };
        }


        /// <summary>
        /// Update related product by Id
        /// </summary>
        /// <param name="id">id of product</param>
        /// <param name="productRequest"></param>
        /// <param name="image">list of product's image</param>
        /// <returns></returns>
        public async Task<BaseResponse<ProductResponse>> UpdateRelatedProduct(string id,
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
                _logger.Error("[ProductService.UpdateRelatedProduct()]" + e.Message);

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
                product.Status = (int)ProductStatus.UNVERIFIED_UPDATE_PRODUCT;
                product.ApproveBy = "";

                _unitOfWork.Repository<Product>().Update(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateRelatedProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
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
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponse
            };
        }


        /// <summary>
        /// Delete Base Product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BaseProductResponse>> DeleteBaseProduct(string id)
        {
            //biz rule

            //validate id
            List<Product> products;
            try
            {
                products = await _unitOfWork.Repository<Product>()
                                           .FindListAsync(p => p.ProductId.Equals(id) || p.BelongTo.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteBaseProduct()]" + e.Message);

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
                products.ForEach(product =>
                {
                    product.Status = (int)ProductStatus.DELETED_PRODUCT;
                    product.UpdatedDate = DateTime.Now;
                    product.ApproveBy = "";

                    _unitOfWork.Repository<Product>().Update(product);
                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteBaseProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //update product in Redis and create response
            List<ProductResponse> productResponses = _mapper.Map<List<ProductResponse>>(products);
            BaseProductResponse baseProductResponse = null;

            productResponses.ForEach(product =>
            {
                _redisService.StoreToList(CACHE_KEY, product,
                    new Predicate<ProductResponse>(p => p.ProductId == product.ProductId));

                if (product.ProductId.Equals(id))
                {
                    baseProductResponse = _mapper.Map<BaseProductResponse>(product);
                }
            });

            productResponses.Remove(productResponses.Find(p => p.ProductId.Equals(id)));
            baseProductResponse.RelatedProducts = productResponses;

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = baseProductResponse
            };
        }


        /// <summary>
        /// Delete Related Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<ProductResponse>> DeleteRelatedProduct(string id)
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
                _logger.Error("[ProductService.DeleteRelatedProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                        ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //update data
            try
            {
                product.UpdatedDate = DateTime.Now;
                product.Status = (int)ProductStatus.DELETED_PRODUCT;
                product.ApproveBy = "";

                _unitOfWork.Repository<Product>().Update(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteRelatedProduct()]" + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Product>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
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
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponse
            };
        }


        /// <summary>
        /// Get Products By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ProductResponse>>> GetProductsByStatus(int status)
        {
            //biz rule


            List<ProductResponse> productResponses;

            //Get Product From Database

            try
            {
                List<Product> products = await _unitOfWork.Repository<Product>().
                                                        FindListAsync(ap => ap.Status == status);

                productResponses = _mapper.Map<List<ProductResponse>>(products);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetProductsByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ProductResponse>
                    {
                        ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                        ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<ProductResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponses
            };
        }


        /// <summary>
        /// Get Products By Product
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ProductResponse>>> GetProductsByProductType(string type)
        {
            //biz rule


            List<ProductResponse> productResponses;

            //Get Product From Database

            try
            {
                List<Product> products = await _unitOfWork.Repository<Product>().
                                                        FindListAsync(ap => ap.ProductType.Equals(type));

                productResponses = _mapper.Map<List<ProductResponse>>(products);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetProductsByProductType()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ProductResponse>
                    {
                        ResultCode = (int)ProductStatus.PRODUCT_NOT_FOUND,
                        ResultMessage = ProductStatus.PRODUCT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<ProductResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponses
            };
        }
    }
}
