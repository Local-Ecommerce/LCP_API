using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly LoichDBContext _context;

        public OrderRepository(LoichDBContext context) : base(context) { }
    }
}