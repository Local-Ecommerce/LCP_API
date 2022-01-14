using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MerchantLevelResponsitory : Repository<MerchantLevel>, IMerchantLevelRepository
    {
        public MerchantLevelResponsitory(LoichDBContext context) : base(context) { }
    }
}