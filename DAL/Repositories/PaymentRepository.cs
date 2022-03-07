using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(LoichDBContext context) : base(context) { }


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
        public async Task<PagingModel<Payment>> GetPayment(
            string id, string orderId,
            string paymentMethodId, DateTime date,
            int?[] status, int? limit,
            int? queryPage, bool isAsc, string propertyName)
        {
            IQueryable<Payment> query = _context.Payments.Where(p => p.PaymentId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(p => p.PaymentId.Equals(id));

            //filter by order id
            if (!string.IsNullOrEmpty(orderId))
                query = query.Where(p => p.OrderId.Equals(orderId));

            //filter by payment method id
            if (!string.IsNullOrEmpty(paymentMethodId))
                query = query.Where(p => p.PaymentMethodId.Equals(paymentMethodId));

            //filter by date
            if (date != DateTime.MinValue)
                query = query.Where(p => p.DateTime.Equals(date.Date));

            //filter by status
            if (status.Length != 0)
                query = query.Where(p => status.Contains(p.Status));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Payment>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}