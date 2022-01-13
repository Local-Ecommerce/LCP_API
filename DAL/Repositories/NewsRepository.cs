using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly LoichDBContext _context;

        public NewsRepository(LoichDBContext context) : base(context) { }
    }
}