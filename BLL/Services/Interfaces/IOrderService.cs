using BLL.Dtos.Order;
using BLL.Dtos.OrderDetail;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// CreateOrder
        /// </summary>
        /// <param name="orderDetailRequests"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<List<ExtendOrderResponse>> CreateOrder(List<OrderDetailRequest> orderDetailRequests, string residentId);


        /// <summary>
        /// Create Order By Market Manager
        /// </summary>
        /// <param name="request"></param>
        /// <param name="marketManagerId"></param>
        /// <returns></returns>
        Task<List<ExtendOrderResponse>> CreateOrderByMarketManager(OrderRequest request, string marketManagerId);


        /// <summary>
        /// Get Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentId"></param>
        /// <param name="role"></param>
        /// <param name="merchantStoreId"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetOrders(
            string id, string residentId,
            string role, string merchantStoreId, int?[] status,
            int? limit, int? page,
            string sort, string include);


        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="role"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task UpdateOrderStatus(string id, int status, string role, string residentId);


        /// <summary>
        /// Caculate Order Detail Final Amount
        /// </summary>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        double? CaculateOrderDetailFinalAmount(double? price, int? quantity, double? discount);


        /// <summary>
        /// Caculate Order Total Amount
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        double? CaculateOrderTotalAmount(OrderDetail orderDetail);
    }
}
