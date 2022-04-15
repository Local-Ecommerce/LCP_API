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
using System.Linq;
using System.Threading.Tasks;
using BLL.Dtos.Product;
using System.Collections.ObjectModel;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly IProductService _productService;
        private readonly IResidentService _residentService;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "OD_";
        private const string SUB_PREFIX = "ODD_";
        private const string CACHE_KEY = "Order";


        public OrderService(ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IRedisService redisService,
            IProductService productService,
            IResidentService residentService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _productService = productService;
            _residentService = residentService;
        }


        /// <summary>
        /// CreateOrder
        /// </summary>
        /// <param name="orderDetailRequests"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<List<ExtendOrderResponse>> CreateOrder(List<OrderDetailRequest> orderDetailRequests, string residentId)
        {
            List<Order> orders = new();
            List<ExtendOrderResponse> extendOrderResponses = new();
            try
            {
                Resident resident = await _unitOfWork.Residents.FindAsync(r => r.ResidentId.Equals(residentId));

                //create new  orders and order details
                foreach (OrderDetailRequest orderDetailRequest in orderDetailRequests)
                {
                    ProductInfoForOrder productInfoForOrder = await _productService
                                                .GetProductPriceForOrder(orderDetailRequest.ProductId);

                    if (productInfoForOrder == null)
                        throw new EntityNotFoundException("Sản phẩm không khả dụng");

                    Order order = orders.Find(o => o.MerchantStoreId.Equals(productInfoForOrder.MerchantStoreId));

                    Collection<OrderDetail> details = order == null ? new Collection<OrderDetail>()
                        : (Collection<OrderDetail>)order.OrderDetails;

                    string orderId = order == null ? _utilService.CreateId(PREFIX) : order.OrderId;

                    //Create order Detail
                    OrderDetail orderDetail = _mapper.Map<OrderDetail>(orderDetailRequest);
                    orderDetail.OrderDetailId = _utilService.CreateId(SUB_PREFIX);
                    orderDetail.OrderId = orderId;
                    orderDetail.UnitPrice = productInfoForOrder.Price;
                    orderDetail.FinalAmount =
                        CaculateOrderDetailFinalAmount(orderDetail.UnitPrice, orderDetail.Quantity, orderDetail.Discount);
                    orderDetail.OrderDate = _utilService.CurrentTimeInVietnam();
                    orderDetail.Status = (int)OrderStatus.OPEN;
                    orderDetail.ProductInMenuId = productInfoForOrder.ProductInMenuId;

                    details.Add(orderDetail);

                    if (order == null)
                    {
                        //create order
                        order = new()
                        {
                            OrderId = orderId,
                            DeliveryAddress = resident.DeliveryAddress,
                            CreatedDate = _utilService.CurrentTimeInVietnam(),
                            UpdatedDate = _utilService.CurrentTimeInVietnam(),
                            Status = orderDetail.Status,
                            Discount = orderDetail.Discount,
                            ResidentId = residentId,
                            MerchantStoreId = productInfoForOrder.MerchantStoreId,
                        };

                        order.OrderDetails = details;
                    }
                }
                //add to db
                foreach (var order in orders)
                {
                    order.TotalAmount = CaculateOrderTotalAmount((Collection<OrderDetail>)order.OrderDetails);
                    _unitOfWork.Orders.Add(order);
                }

                //map to response
                extendOrderResponses = _mapper.Map<List<ExtendOrderResponse>>(orders);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.CreateOrder()]: " + e.Message);
                throw;
            }

            return extendOrderResponses;
        }


        /// <summary>
        /// Create Order By Market Manager
        /// </summary>
        /// <param name="request"></param>
        /// <param name="marketManagerId"></param>
        /// <returns></returns>
        public async Task<List<ExtendOrderResponse>> CreateOrderByMarketManager(OrderRequest request, string marketManagerId)
        {
            List<ExtendOrderResponse> extendOrderResponses = new();

            try
            {
                string apartmentId = (await _unitOfWork.Residents
                                    .FindAsync(r => r.ResidentId.Equals(marketManagerId)))
                                    .ApartmentId;
                string residentId = request.ResidentId != null ? request.ResidentId :
                                await _residentService.CreateGuest(request.Resident, apartmentId, marketManagerId);

                extendOrderResponses = await CreateOrder(request.Products, residentId);
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.CreateOrderByMarketManager()]: " + e.Message);
                throw;
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
            int? page, string sort, string[] include)
        {
            List<ExtendOrderResponse> responses = new();

            //check role
            if (role.Equals(ResidentType.MERCHANT))
            {

                MerchantStore store = (await _unitOfWork.MerchantStores.FindListAsync(ms => ms.ResidentId.Equals(residentId)))
                                        .FirstOrDefault();

                merchantStoreId = store.MerchantStoreId;
            }

            residentId = !role.Equals(ResidentType.CUSTOMER) ? null : residentId;

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

                if (include.Contains("product"))
                {
                    foreach (var order in orders.List)
                    {
                        Collection<OrderDetailResponse> details = new();

                        foreach (var orderDetail in order.OrderDetails)
                        {
                            RelatedProductResponse product = _mapper.Map<RelatedProductResponse>(orderDetail.ProductInMenu.Product);
                            OrderDetailResponse detail = _mapper.Map<OrderDetailResponse>(orderDetail);
                            detail.Product = product;
                            details.Add(detail);
                        }
                        ExtendOrderResponse response = _mapper.Map<ExtendOrderResponse>(order);
                        response.OrderDetails = details;

                        responses.Add(response);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.GetOrder()]" + e.Message);
                throw;
            }

            return new PagingModel<ExtendOrderResponse>
            {
                List = responses,
                Page = orders.Page,
                LastPage = orders.LastPage,
                Total = orders.Total,
            };
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
            return price * quantity - price * quantity * discount;
        }


        /// <summary>
        /// Caculate Order Total Amount
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        public double? CaculateOrderTotalAmount(Collection<OrderDetail> orderDetails)
        {
            double result = 0;
            foreach (var detail in orderDetails)
            {
                result += (double)detail.FinalAmount;
            }
            return result;
        }


        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="role"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task UpdateOrderStatus(string id, int status, string role, string residentId)
        {
            //update order
            try
            {
                Order order = await _unitOfWork.Orders.GetOrder(id);

                //check merchant permission
                if (role.Equals(ResidentType.MERCHANT))
                {
                    MerchantStore store = await _unitOfWork.MerchantStores
                            .FindAsync(ms => ms.MerchantStoreId.Equals(order.MerchantStoreId));

                    if (!store.ResidentId.Equals(residentId))
                        throw new BusinessException("Bạn không thể chỉnh sửa đơn hàng này");
                }
                else
                {
                    //check customer permission
                    if (!order.ResidentId.Equals(residentId))
                        throw new BusinessException("Bạn không thể chỉnh sửa đơn hàng này");
                }

                //check status
                if (!Enum.IsDefined(typeof(OrderStatus), status))
                    throw new BusinessException($"Trạng thái đơn hàng: {status} không khả dụng");

                order.Status = status;
                _unitOfWork.Orders.Update(order);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[OrderService.UpdateOrder()]: " + e.Message);
                throw;
            }
        }

    }
}
