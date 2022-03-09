using BLL.Dtos.ProductCategory;
using DAL.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Create Product Category
        /// </summary>
        /// <param name="product"></param>
        /// <param name="systemCategoryIds"></param>
        /// <returns></returns>
        Product CreateProCategory(Product product, List<string> systemCategoryIds);


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
        /// Delete Product Category By Product Id
        /// </summary>
        /// <param name="productIds"></param>
        Task DeleteProCategoryByProductId(List<string> productIds);


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


        /// <summary>
        /// Verify Product Category
        /// </summary>
        /// <param name="isApprove"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        Product VerifyProCategory(bool isApprove, Product product);
    }
}
