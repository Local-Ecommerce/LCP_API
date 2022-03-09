using BLL.Dtos.Product;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Create a Product
        /// </summary>
        /// <param name="baseProductRequest"></param>
        /// <returns></returns>
        Task<ExtendProductResponse> CreateProduct(string residentId, BaseProductRequest baseProductRequest);


        /// <summary>
        /// Add Related Product
        /// </summary>
        /// <param name="baseProductId"></param>
        /// <param name="residentId"></param>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        Task<PagingModel<ExtendProductResponse>> AddRelatedProduct(string baseProductId, string residentId,
            List<ProductRequest> productRequests);


        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        Task UpdateProduct(List<UpdateProductRequest> productRequests);


        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="apartmentId"></param>
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<ExtendProductResponse>> GetProduct(
            string id, int?[] status, string apartmentId, string type,
            int? limit, int? page,
            string sort, string include);


        /// <summary>
        /// Delete Product by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteProduct(List<string> ids);


        /// <summary>
        /// Verify Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ProductResponse> VerifyProductById(string productId, bool isApprove);
    }
}
