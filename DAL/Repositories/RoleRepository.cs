using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(LoichDBContext context) : base(context) { }
    }
}