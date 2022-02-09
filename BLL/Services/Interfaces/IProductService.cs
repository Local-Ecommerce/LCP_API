using BLL.Dtos;
using BLL.Dtos.Product;
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
        Task<BaseResponse<ExtendProductResponse>> CreateProduct(BaseProductRequest baseProductRequest);


        /// <summary>
        /// Add Related Product
        /// </summary>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> AddRelatedProduct(string baseProductId,
            List<ProductRequest> productRequests);


        /// <summary>
        /// Request Update Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendProductResponse>> RequestUpdateProduct(string id, ProductRequest productRequest);


        /// <summary>
        /// Get All Base Product
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendProductResponse>>> GetAllBaseProduct();


        /// <summary>
        /// Get Base Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendProductResponse>> GetBaseProductById(string id);


        /// <summary>
        /// Get Related Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> GetRelatedProductById(string id);


        /// <summary>
        /// Delete Base Product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendProductResponse>> DeleteBaseProduct(string id);


        /// <summary>
        /// Delete Related Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> DeleteRelatedProduct(string id);


        /// <summary>
        /// Get Products By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ProductResponse>>> GetProductsByStatus(int status);


        /// <summary>
        /// Get Pending Products
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendProductResponse>>> GetPendingProducts();


        /// <summary>
        /// Verify Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> VerifyProductById(string productId, bool isApprove);
    }
}
