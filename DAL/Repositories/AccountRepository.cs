using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(LoichDBContext context) : base(context) { }

        /// <summary>
        /// Get Account Include Resident And Refresh Token By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountIncludeResidentAndRefreshToken(string accountId)
        {
            Account account = await _context.Accounts
                            .Where(acc => acc.AccountId.Equals(accountId))
                            .Include(acc => acc.Residents)
                            .Include(acc => acc.RefreshTokens)
                            .FirstOrDefaultAsync();

            return account;
        }
    }
}