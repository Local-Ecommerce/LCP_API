using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(LoichDBContext context) : base(context) { }


    }
}