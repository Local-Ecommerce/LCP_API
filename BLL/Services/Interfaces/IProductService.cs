using BLL.Dtos;
using BLL.Dtos.Product;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;


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
        BaseResponse<ProductResponse> CreateProduct(ProductRequest productRequest,
            List<IFormFile> image);


        /// <summary>
        /// Update a product by id
        /// </summary>
        /// <param name="id">id of product</param>
        /// <param name="productRequest"></param>
        /// <param name="image">list of product's image</param>
        /// <returns></returns>
        BaseResponse<ProductResponse> UpdateProduct(string id, 
            ProductRequest productRequest, 
            List<IFormFile> image);


        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<ProductResponse> GetProductById(string id);


        /// <summary>
        /// DeleteProduct by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<ProductResponse> DeleteProduct(string id);

        /// <summary>
        /// Store product to Redis
        /// </summary>
        /// <param name="product"></param>
        void StoreProductToRedis(ProductResponse product);
    }
}
