using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Account;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using BLL.Dtos.RefreshToken;
using System.Collections.ObjectModel;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IFirebaseService _firebaseService;
        private readonly IUtilService _utilService;
        private readonly ITokenService _tokenService;
        private const string TOKEN_BLACKLIST_KEY = "Token Blacklist";

        public AccountService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IFirebaseService firebaseService,
            IUtilService utilService,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _firebaseService = firebaseService;
            _tokenService = tokenService;
            _utilService = utilService;
        }

        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<Account> Register(Account account, string uid)
        {
            //Get data from firestore database
            ExtendAccountResponse extendAccountResponse = await _firebaseService.GetUserDataFromFirestoreByUID(uid);
            account = _mapper.Map<Account>(extendAccountResponse);

            return account;
        }


        /// <summary>
        /// Delete Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AccountResponse> DeleteAccount(string id)
        {
            //biz rule

            //valid id
            Account account;
            try
            {
                account = await _unitOfWork.Accounts.FindAsync(a => a.AccountId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.DeleteAccount()]: " + e.Message);
                throw;
            }

            //delete account
            try
            {
                account.UpdatedDate = DateTime.Now;
                account.Status = (int)AccountStatus.DELETED_ACCOUNT;

                _unitOfWork.Accounts.Update(account);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.DeleteAccount()]: " + e.Message);
                throw;
            }

            //revoke token here
            // TokenInfo tokenInfo = new()
            // {
            //     Token = account.Token,
            //     ExpiredDate = account.TokenExpiredDate,
            //     ResidentId = account.RoleId
            // };

            // _redisService.StoreToList<TokenInfo>(TOKEN_BLACKLIST_KEY, tokenInfo,
            //     new Predicate<TokenInfo>(ti => ti.Token == tokenInfo.Token));

            return default;
        }


        /// <summary>
        /// Get Account By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExtendAccountResponse> GetAccountById(string id)
        {
            ExtendAccountResponse extendAccountResponse;

            //get account from database
            try
            {
                Account account = await _unitOfWork.Accounts.FindAsync(acc => acc.AccountId.Equals(id));

                extendAccountResponse = _mapper.Map<ExtendAccountResponse>(account);
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.GetAccountById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Account), id);
            }

            return extendAccountResponse;
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        public async Task<ExtendAccountResponse> Login(AccountRequest accountRequest)
        {
            Account account;
            bool isCreate = false;

            //check valid user
            string uid = await _firebaseService.GetUIDByToken(accountRequest.FirebaseToken);

            try
            {
                //check if user exists
                account = await _unitOfWork.Accounts.GetAccountIncludeResidentAndRefreshToken(uid);

                //create new account if account is not existed yet
                if (account == null)
                {
                    account = await Register(account, uid);
                    isCreate = true;
                }

                string roleId;
                //find resident role
                if (account.RoleId.Equals(RoleId.ADMIN))
                    roleId = account.RoleId;
                else
                {
                    Resident resident = account.Residents.FirstOrDefault();
                    roleId = resident.Type;
                }

                //generate token
                List<RefreshToken> refreshTokens = account.RefreshTokens.ToList();
                if (refreshTokens is null) refreshTokens = new();
                else
                    //revoke old refresh token
                    foreach (RefreshToken rt in refreshTokens)
                        rt.IsRevoked = true;

                refreshTokens.Add(
                    _tokenService.GenerateRefreshToken(account.AccountId, _utilService.CreateId(""), roleId));

                if (isCreate)
                {
                    account.RefreshTokens = refreshTokens;
                    _unitOfWork.Accounts.Add(account);
                }
                else
                {
                    account.RefreshTokens = refreshTokens;
                    _unitOfWork.Accounts.Update(account);
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.Login()]: " + e.Message);

                throw new UnauthorizedAccessException();
            }

            account.RefreshTokens = account.RefreshTokens.Where(rt => rt.IsRevoked == false).ToList();

            //create response
            ExtendAccountResponse response = _mapper.Map<ExtendAccountResponse>(account);
            ExtendRefreshTokenDto refreshToken = response.RefreshTokens.FirstOrDefault();
            refreshToken.AccessTokenExpiredDate = DateTime.Now;

            response.RefreshTokens = new Collection<ExtendRefreshTokenDto>() { refreshToken };

            return response;
        }


        /// <summary>
        /// Update Account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountImageForm"></param>
        /// <returns></returns>
        public async Task<ExtendAccountResponse> UpdateAccount(string id)
        {
            //biz rule

            //valid id
            Account account;
            try
            {
                account = await _unitOfWork.Accounts.FindAsync(a => a.AccountId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.UpdateAccount()]: " + e.Message);

                throw new EntityNotFoundException(typeof(Account), id);
            }

            //update data
            try
            {
                account.UpdatedDate = DateTime.Now;

                _unitOfWork.Accounts.Update(account);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.UpdateAccount()]: " + e.Message);

                throw;
            }

            return _mapper.Map<ExtendAccountResponse>(account);
        }


        /// <summary>
        /// Change Resident Type By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="residentType"></param>
        /// <returns></returns>
        public async Task<ExtendAccountResponse> ChangeResidentTypeByAccountId(string accountId, string residentType)
        {
            // TokenInfo tokenInfo;
            Account account;
            try
            {
                account = await _unitOfWork.Accounts.GetAccountIncludeResidentAndRefreshToken(accountId);

                Resident resident = account.Residents.FirstOrDefault();

                // tokenInfo = new()
                // {
                //     Token = account.Token,
                //     ResidentId = resident.ResidentId,
                //     ExpiredDate = account.TokenExpiredDate
                // };

                // account.Token = null;
                // account.TokenExpiredDate = null;
                // resident.Type = residentType;

                // _unitOfWork.Accounts.Update(account);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.ChangeResidentTypeByAccountId()]: " + e.Message);
                throw;
            }

            //move old token to blacklist
            // _redisService.StoreToList<TokenInfo>(TOKEN_BLACKLIST_KEY, tokenInfo,
            //     new Predicate<TokenInfo>(ti => ti.Token == tokenInfo.Token));

            return _mapper.Map<ExtendAccountResponse>(account);
        }


        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="refreshTokenDto"></param>
        /// <returns></returns>
        public async Task<string> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            RefreshToken refreshToken;
            try
            {
                refreshToken = await _unitOfWork.RefreshTokens.FindAsync(rt => rt.Token.Equals(refreshTokenDto.Token));
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.RefreshToken()]: " + e.Message);
                throw new EntityNotFoundException();
            }

            string accessToken = _tokenService.VerifyAndGenerateToken(refreshTokenDto, refreshToken);

            if (accessToken == null)
                throw new UnauthorizedAccessException();

            try
            {
                _unitOfWork.RefreshTokens.Update(refreshToken);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.RefreshToken()]: " + e.Message);
                throw;
            }

            return accessToken;
        }


        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<string> Logout(string accessToken)
        {
            try
            {
                RefreshToken refreshToken = await _unitOfWork.RefreshTokens.FindAsync(rt => rt.AccessToken.Equals(accessToken));

                refreshToken.IsRevoked = true;

                _unitOfWork.RefreshTokens.Update(refreshToken);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.Logout()]: " + e.Message);
                throw;
            }

            return null;
        }
    }
}
