using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Get Order By Resident Id And Status
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<Order>> GetOrderByResidentIdAndStatus(string residentId, int status);

        /// <summary>
        /// Get Order By Order Id And Resident Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<Order> GetOrderByOrderIdAndResidentId(string orderId, string residentId);


        /// <summary>
        /// Get Orders By Merchant Store Id
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        Task<List<Order>> GetOrdersByMerchantStoreId(string merchantStoreId);
    }
}