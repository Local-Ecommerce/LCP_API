using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="residentId"></param>
        /// <param name="status"></param>
        /// <param name="merchantStoreId"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<Order>> GetOrder(
            string id, string residentId, int?[] status,
            string merchantStoreId, int? limit,
            int? queryPage, bool isAsc,
            string propertyName, string include)
        {
            IQueryable<Order> query = _context.Orders.Where(o => o.OrderId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(o => o.OrderId.Equals(id));

            //filter by residentId
            if (!string.IsNullOrEmpty(residentId))
                query = query.Where(o => o.ResidentId.Equals(residentId));

            //filter by status
            if (status.Length != 0)
                query = query.Where(o => status.Contains(o.Status));

            //filter by merchantStoreId
            if (!string.IsNullOrEmpty(merchantStoreId))
                query = query.Where(o => o.MerchantStoreId.Equals(merchantStoreId));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(propertyName) : query.OrderBy(propertyName + " descending");
            }

            //add include
            if (!string.IsNullOrEmpty(include) && include.Equals("detail"))
            {
                query = query.Include(o => o.OrderDetails);
            }

            //paging
            int perPage = limit.GetValueOrDefault(Int32.MaxValue);
            int page = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            int total = query.Count();

            return new PagingModel<Order>
            {
                List = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
                Total = total,
                Page = page,
                LastPage = (int)Math.Ceiling(total / (double)perPage)
            };
        }

        public async Task<Order> GetOrder(string id, string residentId)
        {
            return await _context.Orders
                                .Where(o => o.OrderId.Equals(id) && o.ResidentId.Equals(residentId))
                                .FirstAsync();
        }
    }
}