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
        /// Get Account Include Resident By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountIncludeResidentByAccountId(string accountId)
        {
            Account account = await _context.Accounts
                            .Where(acc => acc.AccountId.Equals(accountId))
                            .Include(acc => acc.Residents)
                            .FirstOrDefaultAsync();

            return account;
        }


        /// <summary>
        /// Get Account Include Resident By Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountIncludeResidentByUsername(string username)
        {
            Account account = await _context.Accounts
                                        .Where(acc => acc.Username.Equals(username))
                                        .Include(acc => acc.Residents)
                                        .FirstOrDefaultAsync();

            return account;
        }
    }
}