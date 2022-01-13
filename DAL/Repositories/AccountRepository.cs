using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly LoichDBContext _context;

        public AccountRepository(LoichDBContext context) : base(context) { }


    }
}