using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.Exception;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private readonly IProductService _productService;
        private const string CACHE_KEY = "Collection";
        private const string SUB_CACHE_KEY = "CollectionMapping";

        public CollectionService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService,
            IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
            _productService = productService;
        }


        /// <summary>
        /// Create Collection
        /// </summary>
        /// <param name="collectionRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CollectionResponse>> CreateCollection(CollectionRequest collectionRequest)
        {
            //biz rule

            //Store Collection To Dabatabase
            Collection collection = _mapper.Map<Collection>(collectionRequest);

            try
            {
                collection.CollectionId = _utilService.Create16Alphanumeric();
                collection.Status = (int)CollectionStatus.ACTIVE_COLLECTION;
                collection.CreatedDate = DateTime.Now;
                collection.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Collection>().Add(collection);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.CreateCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CollectionResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CollectionResponse collectionResponse = _mapper.Map<CollectionResponse>(collection);

            //Store Collection To Redis
            _redisService.StoreToList(CACHE_KEY, collectionResponse,
                    new Predicate<CollectionResponse>(c => c.CollectionId == collectionResponse.CollectionId));

            return new BaseResponse<CollectionResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponse
            };

        }


        /// <summary>
        /// Delete Collection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BaseResponse<CollectionResponse>> DeleteCollection(string id)
        {
            //biz rule

            //Check id
            Collection collection;
            try
            {
                collection = await _unitOfWork.Repository<Collection>()
                                       .FindAsync(collection => collection.CollectionId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.DeleteCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Collection>
                    {
                        ResultCode = (int)CollectionStatus.COLLECTION_NOT_FOUND,
                        ResultMessage = CollectionStatus.COLLECTION_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete Collection
            try
            {
                collection.Status = (int)CollectionStatus.DELETED_COLLECTION;
                collection.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Collection>().Update(collection);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.DeleteCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Collection>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CollectionResponse collectionResponse = _mapper.Map<CollectionResponse>(collection);

            //Store Collection To Redis
            _redisService.StoreToList(CACHE_KEY, collectionResponse,
                    new Predicate<CollectionResponse>(a => a.CollectionId == collectionResponse.CollectionId));

            return new BaseResponse<CollectionResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponse
            };

        }


        /// <summary>
        /// Get Collection By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CollectionResponse>> GetCollectionById(string id)
        {
            //biz rule


            CollectionResponse collectionResponse = null;
            //Get Collection From Redis
            collectionResponse = _redisService.GetList<CollectionResponse>(CACHE_KEY)
                                            .Find(collection => collection.CollectionId.Equals(id));

            //Get Collection From Database
            if (collectionResponse is null)
            {
                try
                {
                    Collection collection = await _unitOfWork.Repository<Collection>().
                                                            FindAsync(collection => collection.CollectionId.Equals(id));

                    collectionResponse = _mapper.Map<CollectionResponse>(collection);
                }
                catch (Exception e)
                {
                    _logger.Error("[CollectionService.GetCollectionById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<CollectionResponse>
                        {
                            ResultCode = (int)CollectionStatus.COLLECTION_NOT_FOUND,
                            ResultMessage = CollectionStatus.COLLECTION_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<CollectionResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponse
            };
        }


        /// <summary>
        /// Get Collection By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<CollectionResponse>>> GetCollectionByMerchantId(string merchantId)
        {
            List<CollectionResponse> collectionResponses = null;

            //Get Collection From Redis
            collectionResponses = _redisService.GetList<CollectionResponse>(CACHE_KEY)
                                            .Where(collection => collection.MerchantId.Equals(merchantId))
                                            .ToList();

            //Get Collection From Database
            if (collectionResponses is null)
            {
                try
                {
                    List<Collection> collections = await _unitOfWork.Repository<Collection>().
                                                            FindListAsync(collection => collection.MerchantId.Equals(merchantId));

                    collectionResponses = _mapper.Map<List<CollectionResponse>>(collections);
                }
                catch (Exception e)
                {
                    _logger.Error("[CollectionService.GetCollectionByMerchantId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<CollectionResponse>
                        {
                            ResultCode = (int)CollectionStatus.COLLECTION_NOT_FOUND,
                            ResultMessage = CollectionStatus.COLLECTION_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            return new BaseResponse<List<CollectionResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponses
            };
        }


        /// <summary>
        /// Update Collection By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CollectionResponse>> UpdateCollectionById(string id, CollectionRequest collectionRequest)
        {
            //biz ruie

            //Check id
            Collection collection;
            try
            {
                collection = await _unitOfWork.Repository<Collection>()
                                       .FindAsync(collection => collection.CollectionId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.UpdateCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<CollectionResponse>
                {
                    ResultCode = (int)CollectionStatus.COLLECTION_NOT_FOUND,
                    ResultMessage = CollectionStatus.COLLECTION_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update Collection To DB
            try
            {
                collection = _mapper.Map(collectionRequest, collection);
                collection.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Collection>().Update(collection);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.UpdateCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<CollectionResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            CollectionResponse collectionResponse = _mapper.Map<CollectionResponse>(collection);

            //Store Reponse To Redis
            _redisService.StoreToList(CACHE_KEY, collectionResponse,
                    new Predicate<CollectionResponse>(a => a.CollectionId == collectionResponse.CollectionId));

            return new BaseResponse<CollectionResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponse
            };
        }


        /// <summary>
        /// Add Product To Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BaseResponse<CollectionMappingResponse>> AddProductToCollection(
            string collectionId, string productId)
        {
            //biz rule

            //Store CollectionMapping To Dabatabase
            CollectionMapping collectionMapping;
            try
            {
                collectionMapping = new CollectionMapping 
                { 
                    CollectionId = collectionId, 
                    ProductId = productId,
                    Status = (int)CollectionMappingStatus.ACTIVE_PRODUCT
                }; 

                _unitOfWork.Repository<CollectionMapping>().Add(collectionMapping);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.AddProductToCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CollectionResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CollectionMappingResponse collectionMappingResponse = _mapper.Map<CollectionMappingResponse>(collectionMapping);

            //Store CollectionMapping To Redis
            _redisService.StoreToList(SUB_CACHE_KEY, collectionMappingResponse,
                    new Predicate<CollectionMappingResponse>(c =>
                    c.CollectionId.Equals(collectionMappingResponse.CollectionId) ||
                    c.ProductId.Equals(collectionMappingResponse.ProductId)));

            return new BaseResponse<CollectionMappingResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionMappingResponse
            };

        }


        /// <summary>
        /// Remove Product From Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CollectionMappingResponse>> RemoveProductFromCollection(
            string collectionId, string productId)
        {
            //biz rule

            //Get CollectionMapping
            CollectionMapping collectionMapping;
            try
            {
                collectionMapping = await _unitOfWork.Repository<CollectionMapping>()
                                                       .FindAsync(cm =>
                                                       cm.CollectionId.Equals(collectionId) ||
                                                       cm.ProductId.Equals(productId));

            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.RemoveProductFromCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CollectionResponse>
                    {
                        ResultCode = (int)CollectionMappingStatus.PRODUCT_NOT_FOUND,
                        ResultMessage = CollectionMappingStatus.PRODUCT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //remove from database
            try
            {
                _unitOfWork.Repository<CollectionMapping>().Delete(collectionMapping);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.RemoveProductFromCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CollectionResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Remove CollectionMapping From Redis
            _redisService.DeleteFromList(SUB_CACHE_KEY, new Predicate<CollectionMappingResponse>(
                c => c.CollectionId.Equals(collectionId) ||
                c.ProductId.Equals(productId)));

            return new BaseResponse<CollectionMappingResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = _mapper.Map<CollectionMappingResponse>(collectionMapping)
            };

        }


        /// <summary>
        /// Update Product Status In Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CollectionMappingResponse>> UpdateProductStatusInCollection(
            string collectionId, string productId, int status)
        {
            //biz rule

            //Get CollectionMapping
            CollectionMapping collectionMapping;
            try
            {
                collectionMapping = await _unitOfWork.Repository<CollectionMapping>()
                                                       .FindAsync(cm =>
                                                       cm.CollectionId.Equals(collectionId) ||
                                                       cm.ProductId.Equals(productId));

            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.UpdateProductStatusInCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CollectionResponse>
                    {
                        ResultCode = (int)CollectionMappingStatus.PRODUCT_NOT_FOUND,
                        ResultMessage = CollectionMappingStatus.PRODUCT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //update from database
            try
            {
                collectionMapping.Status = status;

                _unitOfWork.Repository<CollectionMapping>().Update(collectionMapping);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.UpdateProductStatusInCollection()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CollectionResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            CollectionMappingResponse collectionMappingResponse = _mapper.Map<CollectionMappingResponse>(collectionMapping);

            //Store CollectionMapping To Redis
            _redisService.StoreToList(SUB_CACHE_KEY, collectionMappingResponse,
                    new Predicate<CollectionMappingResponse>(c =>
                    c.CollectionId.Equals(collectionMappingResponse.CollectionId) ||
                    c.ProductId.Equals(collectionMappingResponse.ProductId)));

            return new BaseResponse<CollectionMappingResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionMappingResponse
            };
        }


        /// <summary>
        /// Get Products By Collection Id
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ProductResponse>>> GetProductsByCollectionId(string collectionId)
        {
            List<CollectionMappingResponse> collectionMappingResponses = null;

            //Get CollectionMapping from Redis
            collectionMappingResponses = _redisService.GetList<CollectionMappingResponse>(SUB_CACHE_KEY)
                                            .Where(cm => cm.CollectionId.Equals(collectionId))
                                            .ToList();

            //Get CollectionMapping from database
            if (collectionMappingResponses == null)
            {
                List<CollectionMapping> collectionMappings;
                try
                {
                    collectionMappings = await _unitOfWork.Repository<CollectionMapping>()
                                                           .FindListAsync(cm => cm.CollectionId.Equals(collectionId));

                    collectionMappingResponses = _mapper.Map<List<CollectionMappingResponse>>(collectionMappings);
                }
                catch (Exception e)
                {
                    _logger.Error("[CollectionService.GetProductsByCollectionId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<CollectionResponse>
                        {
                            ResultCode = (int)CollectionMappingStatus.PRODUCT_NOT_FOUND,
                            ResultMessage = CollectionMappingStatus.PRODUCT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }
            }

            //Get Products
            List<ProductResponse> productResponses = new List<ProductResponse>();

            foreach (CollectionMappingResponse cm in collectionMappingResponses)
            {
                ProductResponse productResponse = _productService.GetProductById(cm.ProductId).Result.Data;

                productResponses.Add(productResponse);
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
