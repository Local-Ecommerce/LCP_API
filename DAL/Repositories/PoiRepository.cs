using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class PoiRepository : Repository<Poi>, IPoiRepository
    {
        private readonly LoichDBContext _context;

        public PoiRepository(LoichDBContext context) : base(context) { }
    }
}