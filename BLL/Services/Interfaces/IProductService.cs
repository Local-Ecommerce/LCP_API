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
        Task<ExtendProductResponse> CreateProduct(BaseProductRequest baseProductRequest);


        /// <summary>
        /// Add Related Product
        /// </summary>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        Task<ExtendProductResponse> AddRelatedProduct(string baseProductId,
            List<ProductRequest> productRequests);


        /// <summary>
        /// Request Update Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<ExtendProductResponse> RequestUpdateProduct(string id, ProductRequest productRequest);


        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<ExtendProductResponse>> GetProduct(
            string id, int?[] status,
            int? limit, int? page,
            string sort, string include);


        /// <summary>
        /// Get Product By Apartment Id And Type
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<ProductResponse>> GetProductByApartmentIdAndType(string apartmentId, string type);


        /// <summary>
        /// Delete Product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExtendProductResponse> DeleteProduct(string id);


        /// <summary>
        /// Verify Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ProductResponse> VerifyProductById(string productId, bool isApprove);
    }
}
