using BLL.Dtos;
using BLL.Dtos.Account;
using DAL.Models;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<Account> Register(Account account, string uid);


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> Login(AccountRequest accountRequest);

        /// <summary>
        /// Get Account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendAccountResponse>> GetAccountById(string id);

        /// <summary>
        /// Update Account
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendAccountResponse>> UpdateAccount(string id);


        /// <summary>
        /// Delete Account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> DeleteAccount(string id);


        /// <summary>
        /// Change Resident Type By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="residentType"></param>
        /// <returns></returns>
        Task<BaseResponse<ExtendAccountResponse>> ChangeResidentTypeByAccountId(string accountId, string residentType);
    }
}
