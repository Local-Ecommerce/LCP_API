using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class StoreMenuDetailRepository : Repository<StoreMenuDetail>, IStoreMenuDetailRepository
    {
        private readonly LoichDBContext _context;

        public StoreMenuDetailRepository(LoichDBContext context) : base(context) { }
    }
}