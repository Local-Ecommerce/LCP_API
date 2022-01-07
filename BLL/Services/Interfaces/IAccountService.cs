using BLL.Dtos;
using BLL.Dtos.Account;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="accountResponse"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> Register(AccountRegisterRequest accountRegisterRequest);


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="accountLoginRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> Login(AccountLoginRequest accountLoginRequest);

        /// <summary>
        /// Get Account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> GetAccountById(string id);

        /// <summary>
        /// Update Account
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> UpdateAccount(string id, AccountImageForm accountImageForm);


        /// <summary>
        /// Delete Account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> DeleteAccount(string id);

        /// <summary>
        /// Check valid confirm password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        bool IsValidConfirmPassword(string password, string confirmPassword);


        /// <summary>
        /// Store Account Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="accountId"></param>
        /// <param name="role"></param>
        void StoreAccountToken(string token, string accountId, string role);


        /// <summary>
        /// Change Role By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<BaseResponse<AccountResponse>> ChangeRoleByAccountId(string accountId, string role);
    }
}
