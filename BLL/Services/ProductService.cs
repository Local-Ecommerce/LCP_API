﻿using AutoMapper;
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
        /// <param name="residentId"></param>
        /// <param name="baseProductRequest"></param>
        /// <returns></returns>
        public async Task<ExtendProductResponse> CreateProduct(string residentId, BaseProductRequest baseProductRequest)
        {
            Product product = _mapper.Map<Product>(baseProductRequest);
            try
            {
                product.ProductId = _utilService.CreateId(PREFIX); ;
                product.Image = _firebaseService
                                        .UploadFilesToFirebase(baseProductRequest.Image, TYPE, product.ProductId, "Image", 0)
                                        .Result;
                product.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;
                product.CreatedDate = DateTime.Now;
                product.UpdatedDate = DateTime.Now;
                product.IsFavorite = 0;
                product.ApproveBy = "";
                product.BelongTo = null;
                product.ResidentId = residentId;
                product.InverseBelongToNavigation = new Collection<Product>();

                //create related product
                foreach (ProductRequest relatedProductRequest in baseProductRequest.RelatedProducts)
                {
                    Product relatedProduct = _mapper.Map<Product>(relatedProductRequest);

                    relatedProduct.ProductId = _utilService.CreateId(PREFIX);
                    relatedProduct.Image = "";
                    relatedProduct.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;
                    relatedProduct.CreatedDate = DateTime.Now;
                    relatedProduct.UpdatedDate = DateTime.Now;
                    relatedProduct.ApproveBy = "";
                    relatedProduct.ResidentId = residentId;
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
        /// <param name="baseProductId"></param>
        /// <param name="residentId"></param>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        public async Task<PagingModel<ExtendProductResponse>> AddRelatedProduct(string baseProductId, string residentId,
            List<ProductRequest> productRequests)
        {
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
                    product.CreatedDate = DateTime.Now;
                    product.UpdatedDate = DateTime.Now;
                    product.ApproveBy = "";
                    product.ResidentId = residentId;
                    product.BelongTo = baseProductId;

                    _unitOfWork.Products.Add(product);
                });

                baseProduct.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;
                _unitOfWork.Products.Update(baseProduct);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.CreateRelatedProduct()]: " + e.Message);
                throw;
            }

            return await GetProduct(baseProductId, Array.Empty<int?>(),
                        default, default, default, default, default, new string[] { "related" });
        }


        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public async Task UpdateProduct(UpdateProductRequest productRequest)
        {
            //get Id of updated product
            List<string> productIds = productRequest.Products.Select(pr => pr.ProductId).ToList();

            //validate ids
            List<Product> products;
            try
            {
                products = await _unitOfWork.Products.FindListAsync(p => productIds.Contains(p.ProductId));

                foreach (var pR in productRequest.Products)
                {
                    //get product from database
                    Product product = products.Where(p => p.ProductId.Equals(pR.ProductId)).FirstOrDefault();

                    product.Status = (int)ProductStatus.UNVERIFIED_PRODUCT;

                    _unitOfWork.Products.Update(product);

                    //get the order of the last photo
                    int order = !string.IsNullOrEmpty(product.Image) ? _utilService.LastImageNumber("Image", product.Image) : 0;

                    //upload new image & remove image
                    string imageUrl = product.Image;
                    if (pR.Image.Length > 0)
                    {
                        foreach (var image in pR.Image)
                        {
                            if (image.Contains("https://firebasestorage.googleapis.com/"))
                                imageUrl = imageUrl.Replace(image + "|", "");
                            else
                                imageUrl += _firebaseService.UploadFilesToFirebase(pR.Image, TYPE, product.ProductId, "Image", order).Result;
                        }
                    }

                    UpdateProductResponse updateProductResponse = _mapper.Map<UpdateProductResponse>(productRequest);
                    updateProductResponse.Image = imageUrl;

                    //store product to Redis
                    _redisService.StoreToList(CACHE_KEY_FOR_UPDATE, updateProductResponse,
                        new Predicate<UpdateProductResponse>(up => up.ProductId.Equals(updateProductResponse.ProductId)));
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
        /// <returns></returns>
        public async Task DeleteProduct(List<string> ids)
        {
            //biz rule

            //validate id
            List<Product> products;
            try
            {
                products = await _unitOfWork.Products
                                .FindListAsync(p => ids.Contains(p.ProductId) || ids.Contains(p.BelongTo));
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.DeleteProduct()]" + e.Message);

                throw new EntityNotFoundException();
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
        }


        /// <summary>
        /// Verify Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ExtendProductResponse> VerifyProductById(string productId, bool isApprove)
        {
            ExtendProductResponse productResponse;

            try
            {
                //get old product from database
                Product baseProduct =
                    (await _unitOfWork.Products
                        .GetProduct(productId, new int?[] { }, null, null, null, null, false, null, new string[] { "related" }))
                        .List
                        .First();

                if (isApprove)
                {
                    //get base product update if available
                    UpdateProductResponse newBaseProduct = _redisService.GetList<UpdateProductResponse>(CACHE_KEY_FOR_UPDATE)
                            .Find(p => p.ProductId == productId);

                    baseProduct = newBaseProduct != null ? _mapper.Map<Product>(newBaseProduct) : baseProduct;

                    //get related product update if available
                    for (int i = 0; i < baseProduct.InverseBelongToNavigation.Count; i++)
                    {
                        Product relatedProduct = baseProduct.InverseBelongToNavigation.ElementAt(i);
                        baseProduct.InverseBelongToNavigation.Remove(relatedProduct);

                        UpdateProductResponse newRelatedProduct = _redisService.GetList<UpdateProductResponse>(CACHE_KEY_FOR_UPDATE)
                                .Find(p => p.ProductId == relatedProduct.ProductId);

                        if (newRelatedProduct != null)
                        {
                            relatedProduct = _mapper.Map<Product>(newRelatedProduct);
                            _redisService.DeleteFromList(CACHE_KEY_FOR_UPDATE,
                                new Predicate<ProductResponse>(p => p.ProductId.Equals(relatedProduct.ProductId)));
                        }

                        baseProduct.InverseBelongToNavigation.Add(relatedProduct);
                    }
                }

                //verify product
                baseProduct.UpdatedDate = DateTime.Now;
                baseProduct.Status = isApprove ? (int)ProductStatus.VERIFIED_PRODUCT : (int)ProductStatus.REJECTED_PRODUCT;
                baseProduct.ApproveBy = isApprove ? "Han" : ""; //update later

                //verify related product
                for (int i = 0; i < baseProduct.InverseBelongToNavigation.Count; i++)
                {
                    Product relatedProduct = baseProduct.InverseBelongToNavigation.ElementAt(i);
                    baseProduct.InverseBelongToNavigation.Remove(relatedProduct);

                    relatedProduct.UpdatedDate = DateTime.Now;
                    relatedProduct.Status = isApprove ? (int)ProductStatus.VERIFIED_PRODUCT : (int)ProductStatus.REJECTED_PRODUCT;
                    relatedProduct.ApproveBy = isApprove ? "Han" : ""; //update later

                    baseProduct.InverseBelongToNavigation.Add(relatedProduct);
                }

                _unitOfWork.Products.Update(baseProduct);

                await _unitOfWork.SaveChangesAsync();

                productResponse = _mapper.Map<ExtendProductResponse>(baseProduct);
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
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<ExtendProductResponse>> GetProduct(
            string id, int?[] status, string apartmentId, string sysCateId,
            int? limit, int? page,
            string sort, string[] include)
        {
            PagingModel<Product> products;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                products = await _unitOfWork.Products.GetProduct
                    (id, status, apartmentId, sysCateId, limit, page, isAsc, propertyName, include);
            }
            catch (Exception e)
            {
                _logger.Error("[ProductService.GetProduct()]" + e.Message);
                throw;
            }

            //get new products if update
            List<ExtendProductResponse> responses = _mapper.Map<List<ExtendProductResponse>>(products.List);

            if (status.Contains((int)ProductStatus.UNVERIFIED_PRODUCT))
            {
                foreach (var response in responses)
                {
                    //get new base product
                    response.UpdatedProduct = _redisService
                            .GetList<UpdateProductResponse>(CACHE_KEY_FOR_UPDATE)
                            .FirstOrDefault(p => p.ProductId == response.ProductId);

                    //get new related product
                    foreach (var related in response.RelatedProducts)
                    {
                        related.UpdatedProduct = _redisService
                                .GetList<UpdateProductResponse>(CACHE_KEY_FOR_UPDATE)
                                .FirstOrDefault(p => p.ProductId == related.ProductId);
                    }
                }
            }

            return new PagingModel<ExtendProductResponse>
            {
                List = responses,
                Page = products.Page,
                LastPage = products.LastPage,
                Total = products.Total,
            };
        }
    }
}
