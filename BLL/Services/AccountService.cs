using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Account;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using BCryptNet = BCrypt.Net.BCrypt;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using BLL.Dtos.JWT;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IRedisService _redisService;
        private readonly IUploadFirebaseService _uploadFirebaseService;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private const string PREFIX = "ACC_";
        private const string TYPE = "Account";
        private const string TOKEN_BLACKLIST_KEY = "Token Blacklist";

        public AccountService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IRedisService redisService,
            IUploadFirebaseService uploadFirebaseService,
            IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _redisService = redisService;
            _uploadFirebaseService = uploadFirebaseService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<AccountResponse>> Register(AccountRegisterRequest accountRegisterRequest)
        {
            //check valid confirm password
            if (!IsValidConfirmPassword(accountRegisterRequest.Password, accountRegisterRequest.ConfirmPassword))
            {
                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)AccountStatus.INVALID_CONFIRM_PASSWORD,
                        ResultMessage = AccountStatus.INVALID_CONFIRM_PASSWORD.ToString(),
                        Data = default
                    });
            }

            string accountId = _utilService.CreateId(PREFIX);

            //upload image
            string profileImageUrl = _uploadFirebaseService
                .UploadFileToFirebase(accountRegisterRequest.ProfileImage, TYPE, accountId, "profileImage").Result;
            string avatarImageUrl = _uploadFirebaseService
                .UploadFileToFirebase(accountRegisterRequest.ProfileImage, TYPE, accountId, "avatarImage").Result;

            //store account to database
            Account account = _mapper.Map<Account>(accountRegisterRequest);
            try
            {
                account.AccountId = accountId;
                account.Password = BCryptNet.HashPassword(accountRegisterRequest.Password);
                account.ProfileImage = profileImageUrl;
                account.AvatarImage = avatarImageUrl;
                account.CreatedDate = DateTime.Now;
                account.UpdatedDate = DateTime.Now;
                account.Status = (int)AccountStatus.ACTIVE_ACCOUNT;

                _unitOfWork.Repository<Account>().Add(account);

                //store account token to database
                StoreAccountToken(null, account.AccountId, Role.Customer);

                await _unitOfWork.SaveChangesAsync();
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
            accountResponse.Role = Role.Customer;

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
                account = await _unitOfWork.Repository<Account>()
                                           .FindAsync(a => a.AccountId.Equals(id));
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

                _unitOfWork.Repository<Account>().Update(account);

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
        public async Task<BaseResponse<AccountResponse>> GetAccountById(string id)
        {
            AccountResponse accountResponse;

                //get account from database
                try
                {
                    await using(var context = new LoichDBContext())
                    {
                        accountResponse = (from acc in context.Accounts
                                    join at in context.AccountTokens
                                    on acc.AccountId equals at.AccountId
                                           where acc.AccountId == id
                                    select new AccountResponse
                                    {
                                        AccountId = acc.AccountId,
                                        Username = acc.Username,
                                        ProfileImage = acc.ProfileImage,
                                        AvatarImage = acc.AvatarImage,
                                        CreatedDate = acc.CreatedDate,
                                        UpdatedDate = acc.UpdatedDate,
                                        Status = acc.Status,
                                        Role = at.Role
                                    }).First();
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("[AccountService.GetAccountById()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<AccountResponse>
                        {
                            ResultCode = (int)AccountStatus.ACCOUNT_NOT_FOUND,
                            ResultMessage = AccountStatus.ACCOUNT_NOT_FOUND.ToString(),
                            Data = default
                        });
                }

            return new BaseResponse<AccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = accountResponse
            };
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="accountLoginRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<AccountResponse>> Login(AccountLoginRequest accountLoginRequest)
        {
            Account account;
            AccountToken accountToken;
            try
            {
                account = await _unitOfWork.Repository<Account>().FindAsync(
                    a => a.Username.Equals(accountLoginRequest.Username));

                if (!BCryptNet.Verify(accountLoginRequest.Password, account.Password))
                {
                    throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)AccountStatus.INVALID_USERNAME_PASSWORD,
                        ResultMessage = AccountStatus.INVALID_USERNAME_PASSWORD.ToString(),
                        Data = default
                    });
                }

                string token = _jwtAuthenticationManager.Authenticate(account, accountLoginRequest.Role);

                accountToken = await _unitOfWork.Repository<AccountToken>()
                    .FindAsync(at => at.AccountId.Equals(account.AccountId));

                accountToken.Token = token;
                accountToken.ExpiredDate = accountLoginRequest.Role.Equals(Role.Admin) 
                                || accountLoginRequest.Role.Equals(Role.MarketManager)
                                ? DateTime.UtcNow.AddHours((double)TimeUnit.ONE_HOUR)
                                : DateTime.UtcNow.AddDays((double)TimeUnit.THIRTY_DAYS);

                _unitOfWork.Repository<AccountToken>().Update(accountToken);

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
            AccountResponse accountResponse = _mapper.Map<AccountResponse>(account);
            accountResponse.Token = accountToken.Token;
            accountResponse.Role = accountToken.Role;

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
        public async Task<BaseResponse<AccountResponse>> UpdateAccount(string id,
            AccountImageForm accountImageForm)
        {
            //biz rule

            //valid id
            Account account;
            try
            {
                account = await _unitOfWork.Repository<Account>()
                                           .FindAsync(a => a.AccountId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.UpdateAccount()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountResponse>
                    {
                        ResultCode = (int)AccountStatus.ACCOUNT_NOT_FOUND,
                        ResultMessage = AccountStatus.ACCOUNT_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //upload image
            string profileImageUrl = accountImageForm.ProfileImage.ToString();
            string avatarImageUrl = accountImageForm.AvatarImage.ToString();

            //update data
            try
            {
                account.ProfileImage = profileImageUrl;
                account.AvatarImage = avatarImageUrl;
                account.UpdatedDate = DateTime.Now;

                _unitOfWork.Repository<Account>().Update(account);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.UpdateAccount()]: " + e.Message);

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
        /// Check valid confirm password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        public bool IsValidConfirmPassword(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
        }


        /// <summary>
        /// Store Account Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="accountId"></param>
        /// <param name="role"></param>
        /// <exception cref="HttpStatusException"></exception>
        public async void StoreAccountToken(string token, string accountId, string role)
        {
            //if token is not null, update exist account token
            if (token != null)
            {
                try
                {
                    AccountToken accountToken = await _unitOfWork.Repository<AccountToken>()
                        .FindAsync(at => at.AccountId == accountId);

                        accountToken.ExpiredDate = role.Equals(Role.Admin) || role.Equals(Role.MarketManager)
                                                    ? DateTime.UtcNow.AddHours((double)TimeUnit.ONE_HOUR)
                                                    : DateTime.UtcNow.AddDays((double)TimeUnit.THIRTY_DAYS);
                        accountToken.Token = token;
                        accountToken.Role = role;

                        _unitOfWork.Repository<AccountToken>().Update(accountToken);
                }
                catch (Exception e)
                {
                    _logger.Error("[AccountService.StoreAccountToken()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK,
                        new BaseResponse<AccountToken>
                        {
                            ResultCode = (int)CommonResponse.ERROR,
                            ResultMessage = CommonResponse.ERROR.ToString(),
                            Data = default
                        });
                }
            }
            //else create new account token
            else
            {
                _unitOfWork.Repository<AccountToken>().Add(new AccountToken
                {
                    TokenId = _utilService.CreateId(PREFIX),
                    Token = token,
                    Role = role,
                    ExpiredDate = null,
                    AccountId = accountId
                }); ;
            }
        }


        /// <summary>
        /// Change Role By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<AccountResponse>> ChangeRoleByAccountId(string accountId, string role)
        {
            AccountToken accountToken;
            TokenInfo tokenInfo;
            try
            {
                accountToken = await _unitOfWork.Repository<AccountToken>()
                    .FindAsync(at => at.AccountId == accountId);

                tokenInfo = new TokenInfo
                {
                    Token = accountToken.Token,
                    Role = accountToken.Role,
                    ExpiredDate = accountToken.ExpiredDate
                };

                accountToken.Token = null;
                accountToken.ExpiredDate = null;
                accountToken.Role = role;

                _unitOfWork.Repository<AccountToken>().Update(accountToken);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[AccountService.ChangeRoleByAccountId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<AccountToken>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //create response
            AccountResponse accountResponse = new AccountResponse
            {
                AccountId = accountId,
                Role = role,
            };

            //move old token to blacklist
            _redisService.StoreToList<TokenInfo>(TOKEN_BLACKLIST_KEY, tokenInfo, 
                new Predicate<TokenInfo>(ti => ti.Token == tokenInfo.Token));

            return new BaseResponse<AccountResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data= accountResponse
            };
        }
    }
}
