using BLL.Dtos.ProductCategory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductCategoryService
    {
        /// <summary>
        /// Create Product Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ProductCategoryResponse> CreateProCategory(ProductCategoryRequest request);


        /// <summary>
        /// Update Product Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ProductCategoryResponse> UpdateProCategory(string id, ProductCategoryRequest request);


        /// <summary>
        /// Delete Product Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductCategoryResponse> DeleteProCategory(string id);


        /// <summary>
        /// Get Product Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExtendProductCategoryResponse> GetProCategoryById(string id);


        /// <summary>
        /// Get Product Categories By Status
        /// </summary>
        /// <returns></returns>
        Task<List<ExtendProductCategoryResponse>> GetProductCategoriesByStatus(int status);
    }
}
