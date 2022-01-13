using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly LoichDBContext _context;

        public MenuRepository(LoichDBContext context) : base(context) { }
    }
}