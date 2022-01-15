using BLL.Dtos;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICollectionService
    {
        /// <summary>
        /// Create Collection
        /// </summary>
        /// <param name="collectionRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<CollectionResponse>> CreateCollection(CollectionRequest collectionRequest);


        /// <summary>
        /// Get Collection By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<CollectionResponse>> GetCollectionById(string id);


        /// <summary>
        /// Update Collection By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<CollectionResponse>> UpdateCollectionById(string id, CollectionRequest collectionRequest);


        /// <summary>
        /// Delete Collection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<CollectionResponse>> DeleteCollection(string id);


        /// <summary>
        /// Get All Collections
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<CollectionResponse>>> GetAllCollections();


        /// <summary>
        /// Add Products To Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<List<CollectionMappingResponse>>> AddProductsToCollection(string collectionId, string[] productIds);


        /// <summary>
        /// Remove Product From Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<CollectionMappingResponse>> RemoveProductFromCollection(string collectionId, string productId);


        /// <summary>
        /// Update Product Status in Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<CollectionMappingResponse>> UpdateProductStatusInCollection(string collectionId, string productId, int status);


        /// <summary>
        /// Get Products By Collection Id
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<BaseProductResponse>>> GetProductsByCollectionId(string collectionId);
    }
}
