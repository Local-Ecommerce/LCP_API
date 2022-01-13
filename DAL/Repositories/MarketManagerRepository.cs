using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MarketManagerRepository : Repository<MarketManager>, IMarketManagerRepository
    {
        private readonly LoichDBContext _context;

        public MarketManagerRepository(LoichDBContext context) : base(context) { }
    }
}