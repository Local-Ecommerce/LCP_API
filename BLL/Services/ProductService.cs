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
using System.Collections.ObjectModel;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUploadFirebaseService _uploadFirebaseService;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PD_";
        private const string TYPE = "Product";
        private const string CACHE_KEY = "Product";



        public ProductService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService,
            IUploadFirebaseService uploadFirebaseService)
        {
            _unitOfWork = unitOfWork;
            _uploadFirebaseService = uploadFirebaseService;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
        }


        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="baseProductRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BaseProductResponse>> CreateProduct(BaseProductRequest baseProductRequest)
        {
            //biz rule

            //store product to database
            Product product = _mapper.Map<Product>(baseProductRequest);
            try
            {
                product.ProductId = _utilService.CreateId(PREFIX); ;
                product.Image = _uploadFirebaseService
                    .UploadFilesToFirebase(baseProductRequest.Image, TYPE, product.ProductId, "Image").Result;
                product.Status = (int)ProductStatus.UNVERIFIED_CREATE_PRODUCT;
                product.CreatedDate = DateTime.Now;
                product.UpdatedDate = DateTime.Now;
                product.IsFavorite = 0;
                product.ApproveBy = "";
                product.BelongTo = null;
                product.InverseBelongToNavigation = new Collection<Product>();

                //create related product
                foreach (ProductRequest relatedProductRequest in baseProductRequest.InverseBelongToNavigation)
                {
                    Product relatedProduct = _mapper.Map<Product>(relatedProductRequest);

                    relatedProduct.ProductId = _utilService.CreateId(PREFIX);
                    relatedProduct.Image = _uploadFirebaseService
                                        .UploadFilesToFirebase(relatedProductRequest.Image, TYPE, relatedProduct.ProductId, "Image")
                                        .Result;
                    relatedProduct.Status = (int)ProductStatus.UNVERIFIED_CREATE_PRODUCT;
                    relatedProduct.CreatedDate = DateTime.Now;
                    relatedProduct.UpdatedDate = DateTime.Now;
                    relatedProduct.ApproveBy = "";
                    relatedProduct.BelongTo = product.ProductId;

                    product.InverseBelongToNavigation.Add(relatedProduct);
                }

                _unitOfWork.Products.Add(product);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateBaseProduct()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ProductResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            BaseProductResponse productResponse = _mapper.Map<BaseProductResponse>(product);

            //store product response to Redis

            // _redisService.StoreToList(CACHE_KEY, productResponse,
            //         new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));


            // //create base product response
            // ProductResponse baseProductResponse = _mapper.Map<ProductResponse>(product);

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponse
            };
        }


        /// <summary>
        /// Add Related Product
        /// </summary>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ProductResponse>> AddRelatedProduct(string baseProductId,
            List<ProductRequest> productRequests)
        {
            try
            {
                productRequests.ForEach(productRequest =>
                {
                    string productId = _utilService.CreateId(PREFIX);

                    //upload image
                    string imageUrl = _uploadFirebaseService
                        .UploadFilesToFirebase(productRequest.Image, TYPE, productId, "Image").Result;

                    Product product = _mapper.Map<Product>(productRequest);

                    product.ProductId = _utilService.CreateId(PREFIX);
                    product.Image = "";
                    product.Status = (int)ProductStatus.UNVERIFIED_CREATE_PRODUCT;
                    product.CreatedDate = DateTime.Now;
                    product.UpdatedDate = DateTime.Now;
                    product.ApproveBy = "";
                    product.BelongTo = baseProductId;

                    _unitOfWork.Products.Add(product);
                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateRelatedProduct()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ProductResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }


            //store new related product to Redis
            // List<ProductResponse> productResponses = _mapper.Map<List<ProductResponse>>(products);
            // productResponses.ForEach(product =>
            // {
            //     _redisService.StoreToList(CACHE_KEY, product,
            //         new Predicate<ProductResponse>(p => p.ProductId == product.ProductId));
            // });

            //create response
            ProductResponse productResponse = GetBaseProductById(baseProductId).Result.Data;

            return new BaseResponse<ProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponse
            };
        }


        /// <summary>
        /// Get Base Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<BaseProductResponse>> GetBaseProductById(string id)
        {
            Product product;

            //get product from database
            try
            {
                product = await _unitOfWork.Products.GetBaseProductById(id);
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

            BaseProductResponse baseProductResponse = _mapper.Map<BaseProductResponse>(product);

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = baseProductResponse
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

            //get product from database
            try
            {

                Product product = await _unitOfWork.Products.GetRelatedProductById(id);

                productResponse = _mapper.Map<ProductResponse>(product);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetRelatedProductById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ProductResponse>
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
                product = await _unitOfWork.Products.FindAsync(p => p.ProductId.Equals(id));
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

                _unitOfWork.Products.Update(product);

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
            // ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
            // _redisService.StoreToList(CACHE_KEY, productResponse,
            //         new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));

            //create response
            BaseProductResponse productResponse = _mapper.Map<BaseProductResponse>(product);

            return new BaseResponse<BaseProductResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponse
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
                product = await _unitOfWork.Products.FindAsync(p => p.ProductId.Equals(id));
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

                _unitOfWork.Products.Update(product);

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
                products = await _unitOfWork.Products
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

                    _unitOfWork.Products.Update(product);
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
                product = await _unitOfWork.Products.FindAsync(p => p.ProductId.Equals(id));
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

                _unitOfWork.Products.Update(product);

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
                List<Product> products = await _unitOfWork.Products.FindListAsync(p => p.Status == status);

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
        /// Get Pending Products
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ProductResponse>>> GetPendingProducts()
        {
            //biz rule

            List<ProductResponse> productResponses;

            //Get Product From Database

            try
            {
                List<Product> products = await _unitOfWork.Products.GetPendingProducts();

                productResponses = _mapper.Map<List<ProductResponse>>(products);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetPendingProducts()]: " + e.Message);

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
