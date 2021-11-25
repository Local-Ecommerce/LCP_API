using BLL.Dtos;
using BLL.Dtos.Product;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <param name="image">list of product's image</param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> CreateProduct(ProductRequest productRequest);


        /// <summary>
        /// Update a product by id
        /// </summary>
        /// <param name="id">id of product</param>
        /// <param name="productRequest"></param>
        /// <param name="image">list of product's image</param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> UpdateProduct(string id, 
            ProductRequest productRequest);


        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> GetProductById(string id);


        /// <summary>
        /// DeleteProduct by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ProductResponse>> DeleteProduct(string id);

    }
}
