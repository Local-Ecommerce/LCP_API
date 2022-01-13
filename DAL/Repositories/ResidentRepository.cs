using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ResidentRepository : Repository<Resident>, IResidentRepository
    {
        private readonly LoichDBContext _context;

        public ResidentRepository(LoichDBContext context) : base(context) { }
    }
}