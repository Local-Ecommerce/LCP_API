using BLL.Dtos;
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
        Task<BaseResponse<ProductCategoryResponse>> CreateProCategory(ProductCategoryRequest request);


        /// <summary>
        /// Update Product Category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductCategoryResponse>> UpdateProCategory(string id, ProductCategoryRequest request);


        /// <summary>
        /// Delete Product Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductCategoryResponse>> DeleteProCategory(string id);


        /// <summary>
        /// Get Product Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendProductCategoryResponse>> GetProCategoryById(string id);


        /// <summary>
        /// Get Product Categories By Status
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<ExtendProductCategoryResponse>>> GetProductCategoriesByStatus(int status);
    }
}
