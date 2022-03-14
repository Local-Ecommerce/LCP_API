using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Order;
using BLL.Dtos.OrderDetail;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "OD_";
        private const string SUB_PREFIX = "ODD_";
        private const string CACHE_KEY = "Order";


        public OrderService(ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IRedisService redisService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }


        /// <summary>
        /// CreateOrder
        /// </summary>
        /// <param name="orderDetailRequests"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<List<ExtendOrderResponse>> CreateOrder(List<OrderDetailRequest> orderDetailRequests, string residentId)
        {
            List<ExtendOrderResponse> extendOrderResponses = new();

            try
            {
                //create new  orders and order details
                foreach (OrderDetailRequest orderDetailRequest in orderDetailRequests)
                {
                    string orderId = _utilService.CreateId(PREFIX);

                    //Create order Detail
                    OrderDetail orderDetail = _mapper.Map<OrderDetail>(orderDetailRequest);
                    orderDetail.OrderDetailId = _utilService.CreateId(SUB_PREFIX);
                    orderDetail.OrderId = orderId;
                    orderDetail.FinalAmount = CaculateOrderDetailFinalAmount(orderDetail.UnitPrice, orderDetail.Quantity, orderDetail.Discount);
                    orderDetail.OrderDate = DateTime.Now;
                    orderDetail.Status = orderDetailRequest.Status;

                    //create order
                    Order order = new()
                    {
                        OrderId = orderId,
                        DeliveryAddress = "",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        TotalAmount = CaculateOrderTotalAmount(orderDetail),
                        Status = orderDetail.Status,
                        Discount = orderDetail.Discount,
                        ResidentId = residentId,
                        MerchantStoreId = orderDetailRequest.MerchantStoreId,
                    };

                    //add to db
                    _unitOfWork.Orders.Add(order);
                    _unitOfWork.OrderDetails.Add(orderDetail);

                    //map to response
                    ExtendOrderResponse extendOrderResponse = _mapper.Map<ExtendOrderResponse>(order);
                    extendOrderResponse.OrderDetails = new Collection<OrderDetailResponse>();
                    extendOrderResponse.OrderDetails.Add(_mapper.Map<OrderDetailResponse>(orderDetail));
                    extendOrderResponses.Add(extendOrderResponse);

                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.CreateOrder()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Order), residentId);
            }

            return extendOrderResponses;
        }


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
        public async Task<object> GetOrders(
            string id, string residentId,
            string role, string merchantStoreId,
            int?[] status, int? limit,
            int? page, string sort, string include)
        {
            //check role
            if (role.Equals(ResidentType.MERCHANT))
            {
                if (merchantStoreId == null)
                {
                    _logger.Error("Merchant without merchant store cannot get order");
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    //Find out if the store belongs to the merchant
                    bool flag = false;
                    foreach (MerchantStore store in await _unitOfWork.MerchantStores.FindListAsync(ms => ms.ResidentId.Equals(residentId)))
                    {
                        if (store.MerchantStoreId.Equals(merchantStoreId)) flag = true;
                        break;
                    }
                    if (!flag)
                    {
                        _logger.Error("The store does not belongs to the merchant");
                        throw new UnauthorizedAccessException();
                    }
                }
            }

            residentId = !residentId.Equals(ResidentType.CUSTOMER) ? null : residentId;

            PagingModel<Order> orders;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                orders = await _unitOfWork.Orders.GetOrder
                        (id, residentId, status, merchantStoreId, limit, page, isAsc, propertyName, include);

                if (_utilService.IsNullOrEmpty(orders.List))
                    throw new EntityNotFoundException(typeof(Order), "in the url");
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.GetOrder()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendOrderResponse>
            {
                List = _mapper.Map<List<ExtendOrderResponse>>(orders.List),
                Page = orders.Page,
                LastPage = orders.LastPage,
                Total = orders.Total,
            };
        }


        /// <summary>
        /// Delete Order By Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task DeleteOrderByOrderId(string orderId, string residentId)
        {
            Order order;
            try
            {
                order = await _unitOfWork.Orders.GetOrder(orderId, residentId);
                order.Status = (int)OrderStatus.DELETED_ORDER;

                OrderDetail orderDetail = order.OrderDetails.FirstOrDefault();
                orderDetail.Status = (int)OrderStatus.DELETED_ORDER;

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.DeleteOrderByOrderIdAndResidentId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Order), orderId);
            }

            //create response
            OrderResponse orderResponse = _mapper.Map<OrderResponse>(order);
        }


        /// <summary>
        /// Caculate Final Amount
        /// </summary>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        public double? CaculateOrderDetailFinalAmount(double? price, int? quantity, double? discount)
        {
            return price - price * discount;
        }


        /// <summary>
        /// Caculate Order Total Amount
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public double? CaculateOrderTotalAmount(OrderDetail orderDetail)
        {
            return orderDetail.FinalAmount * orderDetail.Quantity;
        }


        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<OrderResponse> UpdateOrderStatus(string id, int status)
        {
            Order order;
            try
            {
                order = await _unitOfWork.Orders.FindAsync(o => o.OrderId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.UpdateOrder()]: " + e.Message);
                throw new EntityNotFoundException(typeof(Order), "in the url");
            }

            //check status
            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                _logger.Error($"[OrderService.UpdateOrder()]: Status {status} is invalid");
                throw new IllegalArgumentException();
            }

            //update order
            try
            {
                order.Status = status;
                _unitOfWork.Orders.Update(order);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.UpdateOrder()]: " + e.Message);
                throw;
            }

            return _mapper.Map<OrderResponse>(order);
        }
    }
}
