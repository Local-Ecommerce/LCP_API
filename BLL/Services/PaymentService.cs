﻿using AutoMapper;
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
        /// <exception cref="HttpStatusException"></exception>
        public async Task<PaymentResponse> CreatePayment(PaymentRequest paymentRequest)
        {
            Payment Payment = _mapper.Map<Payment>(paymentRequest);

            try
            {
                Payment.PaymentId = _utilService.CreateId(PREFIX);
                Payment.DateTime = DateTime.Now;
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
        /// <exception cref="HttpStatusException"></exception>
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
        /// Get Payment By Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<PaymentResponse>> GetPaymentByDate(DateTime date)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payment from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(payment => payment.DateTime.Value.Date == date.Date);

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByDate()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Payment), date);
            }

            return paymentResponses;
        }


        /// <summary>
        /// Get Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<PaymentResponse> GetPaymentById(string id)
        {
            PaymentResponse paymentReponse;

            //Get Payment from DB

            try
            {
                Payment payment = await _unitOfWork.Payments.FindAsync(pm => pm.PaymentId.Equals(id));

                paymentReponse = _mapper.Map<PaymentResponse>(payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Payment), id);
            }

            return paymentReponse;
        }


        /// <summary>
        /// Get Payment By Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<PaymentResponse>> GetPaymentByOrderId(string orderId)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payments from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(Payment => Payment.OrderId.Equals(orderId));

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByOrderId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Payment), orderId);
            }

            return paymentResponses;
        }


        /// <summary>
        /// Get Payment By Payment Amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<PaymentResponse>> GetPaymentByPaymentAmount(string amount)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payments from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(pm => pm.PaymentAmount.Equals(amount));

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByPaymentAmount()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Payment), amount);
            }

            return paymentResponses;
        }


        /// <summary>
        /// Get Payment By Payment Method Id
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<PaymentResponse>> GetPaymentByPaymentMethodId(string paymentMethodId)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payments from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(pm => pm.PaymentMethodId.Equals(paymentMethodId));

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByPaymentMethodId()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Payment), paymentMethodId);
            }

            return paymentResponses;
        }


        /// <summary>
        /// Update Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
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
    }
}
