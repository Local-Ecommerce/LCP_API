using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(LoichDBContext context) : base(context) { }
    }
}