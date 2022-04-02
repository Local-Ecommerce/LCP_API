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

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PM_";

        public PaymentService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }


        /// <summary>
        /// Create Payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public async Task<PaymentResponse> CreatePayment(PaymentRequest paymentRequest)
        {
            Payment Payment = _mapper.Map<Payment>(paymentRequest);

            try
            {
                Payment.PaymentId = _utilService.CreateId(PREFIX);
                Payment.DateTime = _utilService.CurrentTimeInVietnam();
                Payment.Status = (int)PaymentStatus.ACTIVE_PAYMENT;

                _unitOfWork.Payments.Add(Payment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.CreatePayment()]: " + e.Message);

                throw;
            }

            return _mapper.Map<PaymentResponse>(Payment);
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
                payment.Status = (int)PaymentStatus.INACTIVE_PAYMENT;

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
