using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        /// <summary>
        /// Get Account Include Resident By Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<Account> GetAccountIncludeResidentByUsername(string username);


        /// <summary>
        /// Get Account Include Resident By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<Account> GetAccountIncludeResidentByAccountId(string accountId);
    }
}