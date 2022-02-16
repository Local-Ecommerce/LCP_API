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
        private const string PREFIX = "PD_";
        private const string TYPE = "Product";
        private const string CACHE_KEY = "Product";
        private const string CACHE_KEY_FOR_UPDATE = "Unverified Updated Product";


        public ProductService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService,
            IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;
            _firebaseService = firebaseService;
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
        public async Task<ExtendProductResponse> CreateProduct(BaseProductRequest baseProductRequest)
        {
            //biz rule

            //store product to database
            Product product = _mapper.Map<Product>(baseProductRequest);
            try
            {
                product.ProductId = _utilService.CreateId(PREFIX); ;
                product.Image = _firebaseService
                                        .UploadFilesToFirebase(baseProductRequest.Image, TYPE, product.ProductId, "Image", 0)
                                        .Result;
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
                    relatedProduct.Image = _firebaseService
                                        .UploadFilesToFirebase(relatedProductRequest.Image, TYPE, relatedProduct.ProductId, "Image", 0)
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

                throw;
            }

            return _mapper.Map<ExtendProductResponse>(product);
        }


        /// <summary>
        /// Add Related Product
        /// </summary>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        public async Task<ProductResponse> AddRelatedProduct(string baseProductId,
            List<ProductRequest> productRequests)
        {
            try
            {
                productRequests.ForEach(productRequest =>
                {
                    string productId = _utilService.CreateId(PREFIX);

                    //upload image
                    string imageUrl = _firebaseService
                        .UploadFilesToFirebase(productRequest.Image, TYPE, productId, "Image", 0).Result;

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

                throw;
            }

            //create response
            ProductResponse productResponse = GetBaseProductById(baseProductId).Result;

            return productResponse;
        }


        /// <summary>
        /// Get Base Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendProductResponse> GetBaseProductById(string id)
        {
            //get product from redis
            ExtendProductResponse baseProductResponse = _redisService.GetList<ExtendProductResponse>(CACHE_KEY)
                .Find(p => p.ProductId.Equals(id));

            if (baseProductResponse == null)
            {
                //get product from database
                try
                {
                    Product product = await _unitOfWork.Products.GetBaseProductById(id);

                    baseProductResponse = _mapper.Map<ExtendProductResponse>(product);
                }
                catch (Exception e)
                {
                    _logger.Error("[ProductService.GetBaseProductById()]: " + e.Message);

                    throw new EntityNotFoundException(typeof(Product), id);
                }
            }

            return baseProductResponse;
        }


        /// <summary>
        /// Get All Base Product
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExtendProductResponse>> GetAllBaseProduct()
        {
            //get products from redis
            List<ExtendProductResponse> extendProductResponses = _redisService.GetList<ExtendProductResponse>(CACHE_KEY);

            if (_utilService.IsNullOrEmpty(extendProductResponses))
            {
                //get products from database
                try
                {
                    List<Product> products = await _unitOfWork.Products.GetAllBaseProduct();

                    extendProductResponses = _mapper.Map<List<ExtendProductResponse>>(products);

                    //store to redis
                    _redisService.StoreList(CACHE_KEY, extendProductResponses);
                }
                catch (Exception e)
                {
                    _logger.Error("[ProductService.GetAllBaseProduct()]: " + e.Message);

                    throw new EntityNotFoundException(typeof(Product), "all");
                }
            }

            return extendProductResponses;
        }


        /// <summary>
        /// Get Related Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductResponse> GetRelatedProductById(string id)
        {
            ProductResponse productResponse;

            //get product from database
            try
            {

                Product product = await _unitOfWork.Products.GetRelatedProductById(id);

                productResponse = _mapper.Map<ProductResponse>(product);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetRelatedProductById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Product), id);
            }

            return productResponse;
        }


        /// <summary>
        /// Request Update product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public async Task<ExtendProductResponse> RequestUpdateProduct(string id, ProductRequest productRequest)
        {
            //validate id
            Product product;
            try
            {
                product = await _unitOfWork.Products.FindAsync(p => p.ProductId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.UpdateProduct()]" + e.Message);

                throw new EntityNotFoundException(typeof(Product), id);
            }

            //get the order of the last photo
            int order = _utilService.LastImageNumber("Image", product.Image);

            //upload image
            string imageUrl = _firebaseService.UploadFilesToFirebase(productRequest.Image, TYPE, product.ProductId, "Image", order)
                                              .Result;

            UpdateProductRequest updateProductRequest = _mapper.Map<UpdateProductRequest>(productRequest);
            updateProductRequest.Image = imageUrl;

            ExtendProductResponse extendProductResponse = _mapper.Map<ExtendProductResponse>(product);
            extendProductResponse.UpdatedProduct = updateProductRequest;

            //store product to Redis
            _redisService.StoreToList(CACHE_KEY_FOR_UPDATE, extendProductResponse,
                new Predicate<ExtendProductResponse>(up => up.ProductId.Equals(extendProductResponse.ProductId)));


            return extendProductResponse;
        }


        /// <summary>
        /// Delete Base Product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendProductResponse> DeleteBaseProduct(string id)
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

                throw new EntityNotFoundException(typeof(Product), id);
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

                throw;
            }

            //update product in Redis and create response
            List<ProductResponse> productResponses = _mapper.Map<List<ProductResponse>>(products);
            ExtendProductResponse extendProductResponse = null;

            productResponses.ForEach(product =>
            {
                _redisService.StoreToList(CACHE_KEY, product,
                    new Predicate<ProductResponse>(p => p.ProductId == product.ProductId));

                if (product.ProductId.Equals(id))
                {
                    extendProductResponse = _mapper.Map<ExtendProductResponse>(product);
                }
            });

            productResponses.Remove(productResponses.Find(p => p.ProductId.Equals(id)));

            return extendProductResponse;
        }


        /// <summary>
        /// Delete Related Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductResponse> DeleteRelatedProduct(string id)
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

                throw new EntityNotFoundException(typeof(Product), id);
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

                throw;
            }

            //create response
            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

            //store product to Redis
            _redisService.StoreToList(CACHE_KEY, productResponse,
                    new Predicate<ProductResponse>(a => a.ProductId == productResponse.ProductId));

            return productResponse;
        }


        /// <summary>
        /// Get Products By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<ProductResponse>> GetProductsByStatus(int status)
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

                throw new EntityNotFoundException(typeof(Product), status);
            }

            return productResponses;
        }


        /// <summary>
        /// Get Pending Products
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExtendProductResponse>> GetPendingProducts()
        {
            List<ExtendProductResponse> productResponses;

            //Get Unverified Create Product From Database
            try
            {
                List<Product> products = await _unitOfWork.Products.GetUnverifiedCreateProducts();

                productResponses = _mapper.Map<List<ExtendProductResponse>>(products);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetPendingProducts()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Product), "pending");
            }

            //Get Unverified Update Product From Redis
            List<ExtendProductResponse> unverifiedUpdateProducts = _redisService.GetList<ExtendProductResponse>(CACHE_KEY_FOR_UPDATE);
            productResponses.AddRange(unverifiedUpdateProducts);

            return productResponses;
        }


        /// <summary>
        /// Verify Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ProductResponse> VerifyProductById(string productId, bool isApprove)
        {
            ProductResponse productResponse;
            bool isUpdate = false;

            try
            {
                //get old product from database
                Product product = await _unitOfWork.Products.FindAsync(p => p.ProductId == productId);

                //get new product from redis
                ExtendProductResponse newProduct = _redisService.GetList<ExtendProductResponse>(CACHE_KEY_FOR_UPDATE)
                    .Find(p => p.ProductId == productId);

                if(newProduct != null)
                {
                    product = _mapper.Map<Product>(newProduct);
                    isUpdate = true;
                }

                product.UpdatedDate = DateTime.Now;
                product.Status = isApprove ? (int)ProductStatus.VERIFIED_PRODUCT : (int)ProductStatus.REJECTED_PRODUCT;
                product.ApproveBy = isApprove ? "Han" : ""; //update later

                _unitOfWork.Products.Update(product);

                await _unitOfWork.SaveChangesAsync();

                productResponse = _mapper.Map<ProductResponse>(product);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.VerifyCreateProductById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Product), productId);
            }

            if(isUpdate)
                //remove from redis
                _redisService.DeleteFromList(CACHE_KEY_FOR_UPDATE,
                    new Predicate<ExtendProductResponse>(p => p.ProductId.Equals(productId)));

            return productResponse;
        }
    }
}
