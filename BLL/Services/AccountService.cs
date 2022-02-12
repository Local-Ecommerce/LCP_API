using AutoMapper;
using DAL.Constants;
using BLL.Dtos;
using BLL.Dtos.Account;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;
using BLL.Dtos.JWT;
using System.Linq;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IFirebaseService _firebaseService;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private const string TOKEN_BLACKLIST_KEY = "Token Blacklist";

        public AccountService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IFirebaseService firebaseService,
            IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _firebaseService = firebaseService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
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
        public async Task<BaseResponse<AccountResponse>> DeleteAccount(string id)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)AccountStatus.ACCOUNT_NOT_FOUND,
                        ResultMessage = AccountStatus.ACCOUNT_NOT_FOUND.ToString(),
                        Data = default
                    });
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //revoke token here
            TokenInfo tokenInfo = new TokenInfo
            {
                Token = account.Token,
                ExpiredDate = account.TokenExpiredDate,
                RoleId = account.RoleId
            };

            _redisService.StoreToList<TokenInfo>(TOKEN_BLACKLIST_KEY, tokenInfo,
                new Predicate<TokenInfo>(ti => ti.Token == tokenInfo.Token));

            return new BaseResponse<AccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = default
            };
        }


        /// <summary>
        /// Get Account By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendAccountResponse>> GetAccountById(string id)
        {
            ExtendAccountResponse ExtendAccountResponse;

            //get account from database
            try
            {
                Account account = await _unitOfWork.Accounts.FindAsync(acc => acc.AccountId.Equals(id));

                ExtendAccountResponse = _mapper.Map<ExtendAccountResponse>(account);
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.GetAccountById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendAccountResponse>
                    {
                        ResultCode = (int)AccountStatus.ACCOUNT_NOT_FOUND,
                        ResultMessage = AccountStatus.ACCOUNT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<ExtendAccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendAccountResponse
            };
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<AccountResponse>> Login(AccountRequest accountRequest)
        {
            DateTime expiredDate;
            Account account;
            bool isCreate = false;

            //check valid user
            string uid = await _firebaseService.GetUIDByToken(accountRequest.FirebaseToken);

            try
            {
                //check if user exists
                account = await _unitOfWork.Accounts.GetAccountIncludeResidentByAccountId(uid);

                //create new account if account is not existed yet
                if(account == null)
                {
                    account = await Register(account, uid);
                    isCreate = true;
                }

                if(account.TokenExpiredDate == null || account.TokenExpiredDate < DateTime.Now)
                {
                    string roleId;
                    //find resident role
                    if (account.RoleId.Equals(RoleId.ADMIN))
                    {
                        //is admin
                        expiredDate = DateTime.Now.AddHours((double)TimeUnit.ONE_HOUR);
                        roleId = account.RoleId;
                    }
                    else
                    {
                        Resident resident = account.Residents.FirstOrDefault();

                        if (resident.Type.Equals(ResidentType.MARKET_MANAGER))
                            expiredDate = DateTime.Now.AddHours((double)TimeUnit.ONE_HOUR);
                        else
                            expiredDate = DateTime.Now.AddDays((double)TimeUnit.THIRTY_DAYS);

                        roleId = resident.Type;
                    }

                    account.Token = _jwtAuthenticationManager.Authenticate(account.AccountId, roleId, expiredDate);
                    account.TokenExpiredDate = expiredDate;

                    if (isCreate)
                        _unitOfWork.Accounts.Add(account);
                    else
                        _unitOfWork.Accounts.Update(account);

                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.Login()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)AccountStatus.UNAUTHORIZED_ACCOUNT,
                        ResultMessage = AccountStatus.UNAUTHORIZED_ACCOUNT.ToString(),
                        Data = default
                    });
            }

            //create response
            AccountResponse accountResponse = _mapper.Map<ExtendAccountResponse>(account);

            return new BaseResponse<AccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = accountResponse
            };
        }


        /// <summary>
        /// Update Account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountImageForm"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<ExtendAccountResponse>> UpdateAccount(string id)
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendAccountResponse>
                    {
                        ResultCode = (int)AccountStatus.ACCOUNT_NOT_FOUND,
                        ResultMessage = AccountStatus.ACCOUNT_NOT_FOUND.ToString(),
                        Data = default
                    });
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

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendAccountResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            ExtendAccountResponse ExtendAccountResponse = _mapper.Map<ExtendAccountResponse>(account);

            return new BaseResponse<ExtendAccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendAccountResponse
            };
        }


        /// <summary>
        /// Change Resident Type By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="residentType"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendAccountResponse>> ChangeResidentTypeByAccountId(string accountId, string residentType)
        {
            TokenInfo tokenInfo;
            Account account;
            try
            {
                account = await _unitOfWork.Accounts.GetAccountIncludeResidentByAccountId(accountId);

                Resident resident = account.Residents.FirstOrDefault();

                tokenInfo = new TokenInfo
                {
                    Token = account.Token,
                    RoleId = resident.ResidentId,
                    ExpiredDate = account.TokenExpiredDate
                };

                account.Token = null;
                account.TokenExpiredDate = null;
                resident.Type = residentType;

                _unitOfWork.Accounts.Update(account);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.ChangeResidentTypeByAccountId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<Account>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            ExtendAccountResponse ExtendAccountResponse = _mapper.Map<ExtendAccountResponse>(account);

            //move old token to blacklist
            _redisService.StoreToList<TokenInfo>(TOKEN_BLACKLIST_KEY, tokenInfo,
                new Predicate<TokenInfo>(ti => ti.Token == tokenInfo.Token));

            return new BaseResponse<ExtendAccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendAccountResponse
            };
        }
    }
}
