using BLL.Dtos.PaymentMethod;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        /// <summary>
        /// Create Payment Method
        /// </summary>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        Task<PaymentMethodResponse> CreatePaymentMethod(PaymentMethodRequest paymentMethodRequest);


        /// <summary>
        /// Get Payment Method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<object> GetPaymentMethods(string id, int?[] status, int? limit, int? page, string sort);


        /// <summary>
        /// Update Payment Method By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        Task<PaymentMethodResponse> UpdatePaymentMethodById(string id, PaymentMethodRequest paymentMethodRequest);


        /// <summary>
        /// Delete Payment Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePaymentMethod(string id);
    }
}
