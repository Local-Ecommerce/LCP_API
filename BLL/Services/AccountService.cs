using AutoMapper;
using DAL.Constants;
using BLL.Dtos;
using BLL.Dtos.Account;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using BCryptNet = BCrypt.Net.BCrypt;
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
        private readonly IUtilService _utilService;
        private readonly IRedisService _redisService;
        private readonly IFirebaseService _uploadFirebaseService;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IValidateDataService _validateDataService;
        private const string PREFIX = "ACC_";
        private const string TYPE = "Account";
        private const string TOKEN_BLACKLIST_KEY = "Token Blacklist";

        public AccountService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IRedisService redisService,
            IFirebaseService uploadFirebaseService,
            IJwtAuthenticationManager jwtAuthenticationManager,
            IValidateDataService validateDataService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _redisService = redisService;
            _uploadFirebaseService = uploadFirebaseService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _validateDataService = validateDataService;
        }

        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<AccountResponse>> Register(AccountRegisterRequest accountRegisterRequest)
        {
            //check valid password
            if (!_validateDataService.IsValidPassword(accountRegisterRequest.Password))
            {
                _logger.Error($"[Invalid Password : '{accountRegisterRequest.Password}']");
                throw new HttpStatusException(HttpStatusCode.OK,
                new BaseResponse<ExtendAccountResponse>
                {
                    ResultCode = (int)AccountStatus.INVALID_PASSWORD,
                    ResultMessage = AccountStatus.INVALID_PASSWORD.ToString(),
                    Data = default
                });
            }

            //check valid confirm password
            if (!IsValidConfirmPassword(accountRegisterRequest.Password, accountRegisterRequest.ConfirmPassword))
            {
                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendAccountResponse>
                    {
                        ResultCode = (int)AccountStatus.INVALID_CONFIRM_PASSWORD,
                        ResultMessage = AccountStatus.INVALID_CONFIRM_PASSWORD.ToString(),
                        Data = default
                    });
            }

            Account account;

            try
            {
                //check if username exists
                account = await _unitOfWork.Accounts.FindAsync(acc => acc.Username.Equals(accountRegisterRequest.Username));
                if (account != null)
                {
                    _logger.Error($"[AccountService.Register()]: Username '{accountRegisterRequest.Username}' is already exists.");

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<AccountResponse>
                        {
                            ResultCode = (int)AccountStatus.ACCOUNT_ALREADY_EXISTS,
                            ResultMessage = AccountStatus.ACCOUNT_ALREADY_EXISTS.ToString(),
                            Data = default
                        });
                }

                string accountId = _utilService.CreateId(PREFIX);

                //upload image
                string profileImageUrl = _uploadFirebaseService
                    .UploadFileToFirebase(accountRegisterRequest.ProfileImage, TYPE, accountId, "profileImage").Result;
                string avatarImageUrl = _uploadFirebaseService
                    .UploadFileToFirebase(accountRegisterRequest.AvatarImage, TYPE, accountId, "avatarImage").Result;

                //store account to database
                account = _mapper.Map<Account>(accountRegisterRequest);

                account.AccountId = accountId;
                account.Password = BCryptNet.HashPassword(accountRegisterRequest.Password);
                account.ProfileImage = profileImageUrl;
                account.AvatarImage = avatarImageUrl;
                account.CreatedDate = DateTime.Now;
                account.UpdatedDate = DateTime.Now;
                account.RoleId = RoleId.APARTMENT;
                account.Token = null;
                account.TokenExpiredDate = null;
                account.Status = (int)AccountStatus.ACTIVE_ACCOUNT;

                _unitOfWork.Accounts.Add(account);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (HttpStatusException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.Register()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            AccountResponse accountResponse = _mapper.Map<AccountResponse>(account);

            return new BaseResponse<AccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = accountResponse
            };
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
        /// <param name="accountLoginRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<AccountResponse>> Login(AccountLoginRequest accountLoginRequest)
        {
            DateTime expiredDate = DateTime.Now;
            string residentId = "";
            string roleId = "";
            Account account;
            try
            {
                account = await _unitOfWork.Accounts.GetAccountIncludeResidentByUsername(accountLoginRequest.Username);

                //check account from db and verify password
                if (!BCryptNet.Verify(accountLoginRequest.Password, account.Password))
                {
                    throw new Exception();
                }

                //find resident role
                if (account.RoleId.Equals(RoleId.ADMIN))
                {
                    //is admin
                    residentId = account.AccountId;
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

                    residentId = resident.ResidentId;
                    roleId = resident.Type;
                }

                account.Token = _jwtAuthenticationManager.Authenticate(residentId, roleId, expiredDate);
                account.TokenExpiredDate = expiredDate;

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.Login()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)AccountStatus.INVALID_USERNAME_PASSWORD,
                        ResultMessage = AccountStatus.INVALID_USERNAME_PASSWORD.ToString(),
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
        /// Check valid confirm password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        public bool IsValidConfirmPassword(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
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
