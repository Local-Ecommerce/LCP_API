using BLL.Dtos.ProductCombination;
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
        /// Get Product Combination
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns> 
        Task<object> GetProductCombination(
            string id, string productId,
            int?[] status, int? limit, int? page,
            string sort);


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
    }
}
