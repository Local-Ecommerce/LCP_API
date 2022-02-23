using System;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        /// <summary>
        /// Get Payment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        /// <param name="paymentMethodId"></param>
        /// <param name="date"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Task<PagingModel<Payment>> GetPayment(
            string id, string orderId, string paymentMethodId,
            DateTime date, int?[] status,
            int? limit, int? queryPage,
            bool isAsc, string propertyName);
    }
}