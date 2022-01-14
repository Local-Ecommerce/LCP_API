using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MerchantRepository : Repository<Merchant>, IMerchantRepository
    {
        public MerchantRepository(LoichDBContext context) : base(context) { }
    }
}
