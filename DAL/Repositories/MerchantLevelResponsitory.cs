using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MerchantLevelResponsitory : Repository<MerchantLevel>, IMerchantLevelRepository
    {
        private readonly LoichDBContext _context;

        public MerchantLevelResponsitory(LoichDBContext context) : base(context) { }
    }
}