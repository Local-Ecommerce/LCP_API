using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>
    {
        /// <summary>
        /// Get Payment Method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Task<PagingModel<PaymentMethod>> GetPaymentMethod(
            string id, int?[] status,
            int? limit, int? queryPage,
            bool isAsc, string propertyName);
    }
}