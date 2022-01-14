using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class DeliveryAddressRepository : Repository<DeliveryAddress>, IDeliveryAddressRepository
    {
        public DeliveryAddressRepository(LoichDBContext context) : base(context) { }
    }
}