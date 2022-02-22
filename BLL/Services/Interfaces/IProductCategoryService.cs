using BLL.Dtos.ProductCategory;
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
        /// Get Product Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<object> GetProCategory(string id, int?[] status, int? limit, int? page, string sort);
    }
}
