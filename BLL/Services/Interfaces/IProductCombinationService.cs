using BLL.Dtos.ProductCombination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductCombinationService
    {
        /// <summary>
        /// Create Product Combination
        /// </summary>
        /// <param name="productCombinationRequest"></param>
        /// <returns></returns>
        Task<ProductCombinationResponse> CreateProductCombination(ProductCombinationRequest productCombinationRequest);


        /// <summary>
        /// Get Product Combination By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductCombinationResponse> GetProductCombinationById(string id);


        /// <summary>
        /// Update Product Combination By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productCombinationRequest"></param>
        /// <returns></returns>
        Task<ProductCombinationResponse> UpdateProductCombinationById(string id, ProductCombinationRequest productCombinationRequest);


        /// <summary>
        /// Delete Product Combination By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductCombinationResponse> DeleteProductCombinationById(string id);


        /// <summary>
        /// Get ProductCombinations By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<ProductCombinationResponse>> GetProductCombinationsByStatus(int status);


        /// <summary>
        /// Get ProductCombinations By Status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ProductCombinationResponse>> GetProductCombinationsByBaseProductId(string id);
        
        
        /// <summary>
        /// Get ProductCombinations By Status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ProductCombinationResponse>> GetProductCombinationsByProductId(string id);
    }
}
