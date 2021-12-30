using BLL.Dtos;
using BLL.Dtos.Payment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Create Payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<PaymentResponse>> CreatePayment(PaymentRequest paymentRequest);


        /// <summary>
        /// //Get Payment by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<PaymentResponse>> GetPaymentById(string id);


        /// <summary>
        /// Get Payment by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BaseResponse<List<PaymentResponse>>> GetPaymentByDate(DateTime date);


        /// <summary>
        /// Get Payment By Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<PaymentResponse>>> GetPaymentByOrderId(string orderId);


        /// <summary>
        /// Get Payment By Payment Method Id
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<PaymentResponse>>> GetPaymentByPaymentMethodId(string paymentMethodId);


        /// <summary>
        /// update Payment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="PaymentRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<PaymentResponse>> UpdatePaymentById(string id, PaymentRequest PaymentRequest);


        /// <summary>
        /// delete Payment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<PaymentResponse>> DeletePaymentById(string id);


        /// <summary>
        /// Get Payment By Payment Amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<BaseResponse<List<PaymentResponse>>> GetPaymentByPaymentAmount(string amount);
    }
}
