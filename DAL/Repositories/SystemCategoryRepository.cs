using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class SystemCategoryRepository : Repository<SystemCategory>, ISystemCategoryRepository
    {
        private readonly LoichDBContext _context;

        public SystemCategoryRepository(LoichDBContext context) : base(context) { }
    }
}