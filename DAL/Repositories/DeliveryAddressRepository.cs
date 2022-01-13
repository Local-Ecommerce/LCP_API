using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class DeliveryAddressRepository : Repository<DeliveryAddress>, IDeliveryAddressRepository
    {
        private readonly LoichDBContext _context;

        public DeliveryAddressRepository(LoichDBContext context) : base(context) { }
    }
}