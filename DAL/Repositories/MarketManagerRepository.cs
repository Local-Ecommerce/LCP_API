using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MarketManagerRepository : Repository<MarketManager>, IMarketManagerRepository
    {
        public MarketManagerRepository(LoichDBContext context) : base(context) { }
    }
}