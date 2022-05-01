using BLL.Dtos.OrderDetail;
using BLL.Dtos.Product;
using DAL.Models;
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
        Task<BaseProductResponse> CreateProduct(string residentId, BaseProductRequest baseProductRequest);


        /// <summary>
        /// Add Related Product
        /// </summary>
        /// <param name="baseProductId"></param>
        /// <param name="residentId"></param>
        /// <param name="productRequests"></param>
        /// <returns></returns>
        Task AddRelatedProduct(string baseProductId, string residentId,
            List<ProductRequest> productRequests);


        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task UpdateProduct(UpdateProductRequest productRequest);


        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="apartmentId"></param>
        /// <param name="sysCateId"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <param name="residentId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<PagingModel<BaseProductResponse>> GetProduct(
            string id, int?[] status, string apartmentId, string sysCateId,
            string search, int? limit, int? page,
            string sort, string[] include, string residentId, string role);


        /// <summary>
        /// Get Unverified Product For MarketManager
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PagingModel<BaseProductResponse>> GetUnverifiedProductForMarketManager(string id);


        /// <summary>
        /// Get Product For Customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentId"></param>
        /// <param name="sysCateId"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<PagingModel<BaseProductResponse>> GetProductForCustomer(
            string id, string residentId, string sysCateId, string search,
            int? limit, int? page);


        /// <summary>
        /// Delete Product by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task DeleteProduct(List<string> ids, string residentId);


        /// <summary>
        /// Verify Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="type"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<BaseProductResponse> VerifyProductById(string productId, bool isApprove, string residentId);


        /// <summary>
        /// Get Product From Menu By SysCateId And ProductId 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sysCateIds"></param>
        /// <param name="menu"></param>
        /// <param name="products"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        List<UpdateProductResponse> GetProductFromMenuBySysCateIdAndProductId(string productId,
            List<string> sysCateIds, Menu menu,
            List<UpdateProductResponse> products, string search);
    }
}
