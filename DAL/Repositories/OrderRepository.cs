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
        /// Get Order By Resident Id And Status
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrderByResidentIdAndStatus(string residentId, int status)
        {
            List<Order> orders = await _context.Orders
                                            .Where(o => o.ResidentId.Equals(residentId) && o.Status.Equals(status))
                                            .Include(o => o.OrderDetails)
                                            .ToListAsync();

            return orders;
        }


        /// <summary>
        /// Get Order By Order Id And Resident Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderByOrderIdAndResidentId(string orderId, string residentId)
        {
            Order order = await _context.Orders
                                    .Where(o => o.ResidentId.Equals(residentId) && o.ResidentId.Equals(residentId))
                                    .Include(o => o.OrderDetails)
                                    .FirstOrDefaultAsync();

            return order;
        }


        /// <summary>
        /// Get Orders By Merchant Store Id
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrdersByMerchantStoreId(string merchantStoreId)
        {
            List<Order> orders = await _context.Orders
                                    .Where(o => o.MerchantStoreId.Equals(merchantStoreId))
                                    .Include(o => o.OrderDetails)
                                    .ToListAsync();

            return orders;
        }
    }
}