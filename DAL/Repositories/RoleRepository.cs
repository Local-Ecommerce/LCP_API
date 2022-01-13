using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly LoichDBContext _context;

        public RoleRepository(LoichDBContext context) : base(context) { }
    }
}