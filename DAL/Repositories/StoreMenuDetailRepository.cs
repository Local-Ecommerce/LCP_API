using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class StoreMenuDetailRepository : Repository<StoreMenuDetail>, IStoreMenuDetailRepository
    {
        public StoreMenuDetailRepository(LoichDBContext context) : base(context) { }
    }
}