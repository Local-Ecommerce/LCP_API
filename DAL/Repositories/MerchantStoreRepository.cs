using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MerchantStoreRepository : Repository<MerchantStore>, IMerchantStoreRepository
    {
        private readonly LoichDBContext _context;

        public MerchantStoreRepository(LoichDBContext context) : base(context) { }
    }
}