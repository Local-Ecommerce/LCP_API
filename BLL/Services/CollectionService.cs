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
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IProductService _productService;
        private const string PREFIX = "CLT_";

        public CollectionService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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
                collection.CollectionId = _utilService.CreateId(PREFIX);
                collection.Status = (int)CollectionStatus.ACTIVE_COLLECTION;
                collection.CreatedDate = DateTime.Now;
                collection.UpdatedDate = DateTime.Now;

                _unitOfWork.Collections.Add(collection);

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
                collection = await _unitOfWork.Collections
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

                _unitOfWork.Collections.Update(collection);

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

            Collection collection;
            //Get Collection From Database

            try
            {
                collection = await _unitOfWork.Collections.GetCollectionIncludeResidentByCollectionId(id);
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

            CollectionResponse collectionResponse = _mapper.Map<CollectionResponse>(collection);

            return new BaseResponse<CollectionResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponse
            };
        }


        /// <summary>
        /// Update Collection By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionUpdateRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<CollectionResponse>> UpdateCollectionById(string id,
            CollectionUpdateRequest collectionUpdateRequest)
        {
            //Check id
            Collection collection;
            try
            {
                collection = await _unitOfWork.Collections
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
                collection = _mapper.Map(collectionUpdateRequest, collection);
                collection.UpdatedDate = DateTime.Now;

                _unitOfWork.Collections.Update(collection);

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

            return new BaseResponse<CollectionResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponse
            };
        }


        /// <summary>
        /// Add Products To Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<CollectionMappingResponse>>> AddProductsToCollection(
            string collectionId, string[] productIds)
        {
            //biz rule

            //Store CollectionMapping To Dabatabase
            List<CollectionMapping> collectionMappings = new List<CollectionMapping>();
            try
            {
                foreach (string productId in productIds)
                {
                    CollectionMapping collectionMapping = new CollectionMapping
                    {
                        CollectionId = collectionId,
                        ProductId = productId,
                        Status = (int)CollectionMappingStatus.ACTIVE_PRODUCT
                    };

                    collectionMappings.Add(collectionMapping);
                    _unitOfWork.CollectionMappings.Add(collectionMapping);
                }

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
            List<CollectionMappingResponse> collectionMappingResponses = _mapper.Map<List<CollectionMappingResponse>>(collectionMappings);

            return new BaseResponse<List<CollectionMappingResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionMappingResponses
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
                collectionMapping = await _unitOfWork.CollectionMappings
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
                _unitOfWork.CollectionMappings.Delete(collectionMapping);

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
                collectionMapping = await _unitOfWork.CollectionMappings
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

                _unitOfWork.CollectionMappings.Update(collectionMapping);

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
        public async Task<BaseResponse<List<BaseProductResponse>>> GetProductsByCollectionId(string collectionId)
        {
            List<CollectionMappingResponse> collectionMappingResponses;


            //Get CollectionMappings from database

            List<CollectionMapping> collectionMappings;
            try
            {
                collectionMappings = await _unitOfWork.CollectionMappings
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

            //Get Products
            List<BaseProductResponse> productResponses = new List<BaseProductResponse>();

            foreach (CollectionMappingResponse cm in collectionMappingResponses)
            {
                BaseProductResponse productResponse = _productService.GetBaseProductById(cm.ProductId).Result.Data;

                productResponses.Add(productResponse);
            }

            return new BaseResponse<List<BaseProductResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = productResponses
            };

        }


        /// <summary>
        /// Get All Collections
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<CollectionResponse>>> GetAllCollections()
        {
            List<Collection> collections;


            //Get Collections From Database
            try
            {
                collections = await _unitOfWork.Collections.GetAllCollectionsIncludeResident();
            }
            catch (Exception e)
            {
                _logger.Error("[CollectionService.GetAllCollections()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<CollectionResponse>
                    {
                        ResultCode = (int)CollectionStatus.COLLECTION_NOT_FOUND,
                        ResultMessage = CollectionStatus.COLLECTION_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            List<CollectionResponse> collectionResponses = _mapper.Map<List<CollectionResponse>>(collections);


            return new BaseResponse<List<CollectionResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = collectionResponses
            };
        }
    }
}
