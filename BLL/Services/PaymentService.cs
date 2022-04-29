using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.Payment;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Dtos.MoMo.CaptureWallet;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IFirebaseService _firebaseService;
        private readonly IMoMoService _moMoService;
        private const string PREFIX = "PM_";
        private const string MOMO = "PM_MOMO";
        private const string CASH = "PM_CASH";

        public PaymentService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IConfiguration configuration,
            ISecurityService securityService,
            IMoMoService moMoService,
            IFirebaseService firebaseService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _configuration = configuration;
            _securityService = securityService;
            _firebaseService = firebaseService;
            _moMoService = moMoService;
        }


        /// <summary>
        /// Create Payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public async Task<PaymentLinkResponse> CreatePayment(PaymentRequest paymentRequest)
        {
            PaymentLinkResponse response = null;

            try
            {
                //check payment amount
                Order order = (await _unitOfWork.Orders.GetOrder(paymentRequest.OrderId, null, null, null, null, null, false, null, new string[] { "product" }))
                    .List
                    .First();

                if (order.TotalAmount != paymentRequest.PaymentAmount)
                    throw new BusinessException("Số tiền thanh toán không trùng khớp với giá trị đơn hàng");

                //if payment method is MoMo
                if (paymentRequest.PaymentMethodId.Equals(MOMO))
                {
                    MoMoCaptureWalletRequest momoRequest = new MoMoCaptureWalletRequest
                    {
                        PartnerCode = _configuration.GetValue<string>("MoMo:PartnerCode"),
                        RequestId = Guid.NewGuid().ToString(),
                        Amount = Convert.ToInt64(paymentRequest.PaymentAmount),
                        OrderId = paymentRequest.OrderId,
                        OrderInfo = $"Thanh toán đơn hàng {paymentRequest.OrderId} từ LCP",
                        RedirectUrl = paymentRequest.RedirectUrl,
                        // IpnUrl = "https://localcommercialplatform-api.azurewebsites.net/api/ipn",
                        IpnUrl = "https://eeb8-171-240-159-178.ap.ngrok.io/api/ipn",
                        RequestType = "captureWallet",
                        ExtraData = "",
                        StoreId = "Test_01"
                    };

                    // Validate signature
                    List<string> ignoreFields = new List<string>() { "Signature", "PartnerName", "StoreId", "Lang", "ToJson" };

                    string merchantSignature = _securityService.GetSignature(momoRequest, ignoreFields,
                        _configuration.GetValue<string>("MoMo:AccessKey"), _configuration.GetValue<string>("MoMo:SecretKey"));

                    momoRequest.Signature = merchantSignature;

                    MoMoCaptureWalletResponse momoResponse = _moMoService.CreateCaptureWallet(momoRequest);

                    response = new PaymentLinkResponse { PayUrl = momoResponse.payUrl };
                }

                Payment payment = _mapper.Map<Payment>(paymentRequest);

                payment.PaymentId = _utilService.CreateId(PREFIX);
                payment.DateTime = _utilService.CurrentTimeInVietnam();
                payment.Status = (int)PaymentStatus.UNPAID;

                _unitOfWork.Payments.Add(payment);

                await _unitOfWork.SaveChangesAsync();

                //push notification
                Product product = order.OrderDetails
                    .First()
                    .ProductInMenu
                    .Product;

                await _firebaseService.PushNotification(order.ResidentId, product.ResidentId, product.Image, $"{(int)NotificationCode.PAYMENT}");
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.CreatePayment()]: " + e.Message);
                throw;
            }

            return response;
        }


        /// <summary>
        /// Get Payment
        /// </summary>        
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<object> GetPayments(
            string id, string orderId,
            string paymentMethodId, DateTime date,
            int?[] status, int? limit,
            int? page, string sort)
        {
            PagingModel<Payment> paymentMethods;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                paymentMethods = await _unitOfWork.Payments
                .GetPayment(id, orderId, paymentMethodId, date, status, limit, page, isAsc, propertyName);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPayment()]" + e.Message);
                throw;
            }

            return new PagingModel<PaymentResponse>
            {
                List = _mapper.Map<List<PaymentResponse>>(paymentMethods.List),
                Page = paymentMethods.Page,
                LastPage = paymentMethods.LastPage,
                Total = paymentMethods.Total,
            };
        }


        /// <summary>
        /// Create Payment ForGuest
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="orderId"></param>
        public void CreatePaymentForGuest(double? amount, string orderId)
        {
            Payment Payment = new()
            {
                PaymentId = _utilService.CreateId(PREFIX),
                PaymentAmount = amount,
                DateTime = _utilService.CurrentTimeInVietnam(),
                Status = (int)PaymentStatus.UNPAID,
                OrderId = orderId,
                PaymentMethodId = "PM_CASH"
            };

            _unitOfWork.Payments.Add(Payment);
        }
    }
}
