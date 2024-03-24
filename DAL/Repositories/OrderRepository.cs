using System;
using System.Collections.Generic;
using System.Linq;
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
            string propertyName, string[] include)
        {
            IQueryable<Order> query = _context.Orders.Where(o => o.OrderId != null);

            //filter by id
            if (!string.IsNullOrEmpty(id))
                query = query.Where(o => o.OrderId.Equals(id));

            //filter by residentId
            if (!string.IsNullOrEmpty(residentId))
                query = query.Where(o => o.ResidentId.Equals(residentId));

            //filter by status
            if (status != null && status.Length != 0)
                query = query.Where(o => status.Contains(o.Status));

            //filter by merchantStoreId
            if (!string.IsNullOrEmpty(merchantStoreId))
                query = query.Where(o => o.MerchantStoreId.Equals(merchantStoreId));

            //sort
            if (!string.IsNullOrEmpty(propertyName))
            {
                query = isAsc ? query.OrderBy(c => propertyName) : query.OrderBy(c => propertyName + " descending");
            }

            //add include
            if (include != null && include.Length > 0)
            {
                foreach (string item in include)
                {
                    switch (item)
                    {
                        case "detail":
                            query = query.Include(o => o.OrderDetails);
                            break;
                        case "product":
                            query = query.Include(o => o.OrderDetails)
                                            .ThenInclude(od => od.ProductInMenu)
                                            .ThenInclude(pim => pim.Product)
                                            .ThenInclude(p => p.BelongToNavigation);
                            break;
                        case "resident":
                            query = query.Include(o => o.Resident);
                            break;
                        case "payment":
                            query = query.Include(o => o.Payments);
                            break;
                    }
                }
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


        /// <summary>
        /// Get Order By Order Ids
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrderByOrderIds(List<string> orderIds)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                                            .ThenInclude(od => od.ProductInMenu)
                                            .ThenInclude(pim => pim.Product)
                                            .ThenInclude(p => p.BelongToNavigation)
                                            .Where(o => orderIds.Contains(o.OrderId))
                                            .ToListAsync();
        }


        /// <summary>
        /// Get Order For Dashboard
        /// </summary>
        /// <param name="days"></param>
        /// <param name="residentId"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrderForDashboard(int days, string residentId, string apartmentId)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, vnZone);
            DateTime previousDate = currentDate.Subtract(new TimeSpan(days, 0, 0, 0));

            IQueryable<Order> query = _context.Orders.Where(o => o.OrderId != null);

            //get by days
            query = query
                .Where(o => o.UpdatedDate.Value.Date <= currentDate.Date && o.UpdatedDate.Value.Date >= previousDate.Date);

            //get by residentId
            if (residentId != null)
                query = query.Include(o => o.MerchantStore).Where(o => o.MerchantStore.ResidentId.Equals(residentId));

            //get by apartmentId
            if (apartmentId != null)
            {
                query = apartmentId != "" ? query.Include(o => o.Resident)
                                                .Where(o => o.Resident.ApartmentId.Equals(apartmentId)) :
                                        query.Include(o => o.Resident);
            }

            query = query.Include(o => o.Payments).Where(o => o.Payments.Any());

            return await query.ToListAsync();
        }
    }
}