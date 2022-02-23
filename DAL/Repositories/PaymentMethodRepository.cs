using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(LoichDBContext context) : base(context) { }


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
        public async Task<PagingModel<PaymentMethod>> GetPaymentMethod(
            string id, int?[] status,
            int? limit, int? queryPage,
            bool isAsc, string propertyName)
        {
            IQueryable<PaymentMethod> query = _context.PaymentMethods.Where(p => p.PaymentMethodId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(p => p.PaymentMethodId.Equals(id));

            //filter by status
            if (status.Length != 0)
                query = query.Where(p => status.Contains(p.Status));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //paging
            int perPage = limit.GetValueOrDefault(10);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<PaymentMethod>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }
    }
}