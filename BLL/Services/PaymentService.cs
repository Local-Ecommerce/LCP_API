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
        private readonly IMoMoService _moMoService;
        private const string PREFIX = "PM_";
        private const string MOMO = "PM_MOMO";

        public PaymentService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IConfiguration configuration,
            ISecurityService securityService,
            IMoMoService moMoService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _configuration = configuration;
            _securityService = securityService;
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
                Order order = await _unitOfWork.Orders.FindAsync(o => o.OrderId.Equals(paymentRequest.OrderId));
                if (order.TotalAmount != paymentRequest.PaymentAmount)
                    throw new BusinessException("Số tiền thanh toán không trùng khớp với giá trị đơn hàng");

                //if payment method is MoMo
                if (paymentRequest.PaymentMethodId.Equals(MOMO))
                {
                    MoMoCaptureWalletRequest momoRequest = new MoMoCaptureWalletRequest
                    {
                        PartnerCode = _configuration.GetValue<string>("MoMo:PartnerCode"),
                        RequestId = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                        Amount = Convert.ToInt64(paymentRequest.PaymentAmount),
                        OrderId = paymentRequest.OrderId,
                        OrderInfo = $"Thanh toán đơn hàng {paymentRequest.OrderId} từ LCP",
                        RedirectUrl = paymentRequest.RedirectUrl,
                        IpnUrl = "https://localcommercialplatform-api.azurewebsites.net/api/ipn",
                        RequestType = "captureWallet",
                        ExtraData = ""
                    };

                    // Validate signature
                    List<string> ignoreFields = new List<string>() { "signature", "partnerName", "storeId", "lang" };

                    string rawData = _securityService.GetRawDataSignature(momoRequest, ignoreFields);

                    rawData = "accessKey=" + _configuration.GetValue<string>("MoMo:AccessKey") + "&" + rawData;

                    string merchantSignature = _securityService.SignHmacSHA256(rawData, _configuration.GetValue<string>("MoMo:SecretKey"));

                    momoRequest.Signature = merchantSignature;

                    MoMoCaptureWalletResponse momoResponse = _moMoService.CreateCaptureWallet(momoRequest);

                    response = new PaymentLinkResponse
                    {
                        Deeplink = momoResponse.Deeplink,
                        PayUrl = momoResponse.PayUrl
                    };
                }

                Payment Payment = _mapper.Map<Payment>(paymentRequest);

                Payment.PaymentId = _utilService.CreateId(PREFIX);
                Payment.DateTime = _utilService.CurrentTimeInVietnam();
                Payment.Status = (int)PaymentStatus.UNPAID;

                _unitOfWork.Payments.Add(Payment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.CreatePayment()]: " + e.Message);
                throw;
            }

            return response;
        }


        /// <summary>
        /// Delete Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PaymentResponse> DeletePaymentById(string id)
        {
            //Check id
            Payment payment;
            try
            {
                payment = await _unitOfWork.Payments.FindAsync(pm => pm.PaymentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.DeletePaymentById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Payment), id);
            }

            //Delete Payment
            try
            {
                payment.Status = (int)PaymentStatus.UNPAID;

                _unitOfWork.Payments.Update(payment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.DeletePaymentById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<PaymentResponse>(payment);
        }


        /// <summary>
        /// Update Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public async Task<PaymentResponse> UpdatePaymentById(string id, PaymentRequest paymentRequest)
        {
            Payment payment;
            try
            {
                payment = await _unitOfWork.Payments.FindAsync(pm => pm.PaymentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.UpdatePaymentById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Payment), id);
            }

            //Update Payment to DB
            try
            {
                payment = _mapper.Map(paymentRequest, payment);

                _unitOfWork.Payments.Update(payment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.UpdatePaymentById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<PaymentResponse>(payment);
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
    }
}
