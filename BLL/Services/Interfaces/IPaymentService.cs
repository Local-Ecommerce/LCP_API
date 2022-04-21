using BLL.Dtos.Payment;
using System;
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
        Task<PaymentLinkResponse> CreatePayment(PaymentRequest paymentRequest);


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
        Task<object> GetPayments(
            string id, string orderId,
            string paymentMethodId, DateTime date,
            int?[] status, int? limit,
            int? page, string sort);
    }
}
