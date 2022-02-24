using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(LoichDBContext context) : base(context) { }
    }
}