using BLL.Dtos;
using BLL.Dtos.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Create a Base Product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<BaseProductResponse>> CreateBaseProduct(ProductRequest productRequest);


        /// <summary>
        /// Create Related Product
        /// </summary>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        Task<BaseResponse<BaseProductResponse>> CreateRelatedProduct(string baseProductId, 
            List<ProductRequest> productRequests);


        /// <summary>
        /// Update Base Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<BaseProductResponse>> UpdateBaseProduct(string id, 
            ProductRequest productRequest);


        /// <summary>
        /// Update Related Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> UpdateRelatedProduct(string id,
            ProductRequest productRequest);


        /// <summary>
        /// Get Base Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<BaseProductResponse>> GetBaseProductById(string id);


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
        Task<BaseResponse<BaseProductResponse>> DeleteBaseProduct(string id);


        /// <summary>
        /// Delete Related Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> DeleteRelatedProduct(string id);


        /// <summary>
        /// Get Related Products By Base Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ProductResponse>>> GetRelatedProductsByBaseProductId(string id);


        /// <summary>
        /// Get Products By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ProductResponse>>> GetProductsByStatus(int status);


        /// <summary>
        /// Get Products By Product Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<BaseResponse<List<ProductResponse>>> GetProductsByProductType(string type);
    }
}
