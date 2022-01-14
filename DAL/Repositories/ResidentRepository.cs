using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class ResidentRepository : Repository<Resident>, IResidentRepository
    {
        public ResidentRepository(LoichDBContext context) : base(context) { }
    }
}