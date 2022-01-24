using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Get Base Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<Product> GetBaseProductById(string productId);

        /// <summary>
        /// Get Related Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<Product> GetRelatedProductById(string productId);


        /// <summary>
        /// Get All Base Product
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAllBaseProduct();


        /// <summary>
        /// Get Pending Products
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetPendingProducts();
    }
}