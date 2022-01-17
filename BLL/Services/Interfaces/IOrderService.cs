using BLL.Dtos;
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
        Task<BaseResponse<List<OrderResponse>>> CreateOrder(List<OrderDetailRequest> orderDetailRequests, string residentId);


        /// <summary>
        /// Get Order By Resident Id And Status
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<OrderResponse>>> GetOrderByResidentIdAndStatus(string residentId, int status);


        /// <summary>
        /// Get Order By Merchant Store Id
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<OrderResponse>>> GetOrderByMerchantStoreId(string merchantStoreId);


        /// <summary>
        /// Delete Order By Order Id And Resident Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<BaseResponse<OrderResponse>> DeleteOrderByOrderIdAndResidentId(string orderId, string residentId);


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
