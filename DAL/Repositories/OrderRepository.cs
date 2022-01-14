using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(LoichDBContext context) : base(context) { }
    }
}