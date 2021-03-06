using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Get Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentId"></param>
        /// <param name="status"></param>
        /// <param name="merchantStoreId"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<Order>> GetOrder(
            string id, string residentId, int?[] status,
            string merchantStoreId,
            int? limit, int? queryPage,
            bool isAsc, string propertyName,
            string[] include);


        /// <summary>
        /// Get Order By Order Ids
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        Task<List<Order>> GetOrderByOrderIds(List<string> orderIds);


        /// <summary>
        /// Get Order For Dashboard
        /// </summary>
        /// <param name="days"></param>
        /// <param name="residentId"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        Task<List<Order>> GetOrderForDashboard(int days, string residentId, string apartmentId);
    }
}