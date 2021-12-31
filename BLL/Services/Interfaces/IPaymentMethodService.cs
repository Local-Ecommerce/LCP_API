using BLL.Dtos;
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
        Task<BaseResponse<PaymentMethodResponse>> CreatePaymentMethod(PaymentMethodRequest paymentMethodRequest);


        /// <summary>
        /// Get Payment Method By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<PaymentMethodResponse>> GetPaymentMethodById(string id);


        /// <summary>
        /// Update Payment Method By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<PaymentMethodResponse>> UpdatePaymentMethodById(string id, PaymentMethodRequest paymentMethodRequest);


        /// <summary>
        /// Delete Payment Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<PaymentMethodResponse>> DeletePaymentMethod(string id);


        /// <summary>
        /// Get Payment Method By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BaseResponse<List<PaymentMethodResponse>>> GetAllPaymentMethod();
    }
}
