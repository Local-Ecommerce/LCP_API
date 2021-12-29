using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Account;
using BLL.Dtos.Exception;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private const string CACHE_KEY = "Account";

        public AccountService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService,
            IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<AccountResponse>> Register(AccountRegisterRequest accountRegisterRequest)
        {
            //biz rule
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

            //upload image
            string profileImageUrl = accountRegisterRequest.ProfileImage.ToString();
            string avatarImageUrl = accountRegisterRequest.AvatarImage.ToString();

            //store account to database
            Account account = _mapper.Map<Account>(accountRegisterRequest);
            try
            {
                account.AccountId = _utilService.Create16Alphanumeric();
                account.ProfileImage = profileImageUrl;
                account.AvatarImage = avatarImageUrl;
                account.CreatedDate = DateTime.Now;
                account.UpdatedDate = DateTime.Now;
                account.Status = (int)AccountStatus.ACTIVE_ACCOUNT;

                _unitOfWork.Repository<Account>().Add(account);

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
                    Account account = await _unitOfWork.Repository<Account>()
                                                       .FindAsync(a => a.AccountId.Equals(id));
                    accountResponse = _mapper.Map<AccountResponse>(account);

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

            try
            {
                account = await _unitOfWork.Repository<Account>().FindAsync(
                    a => a.Username.Equals(accountLoginRequest.Username) ||
                    a.Password.Equals(accountLoginRequest.Password));
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
            accountResponse.Token = _jwtAuthenticationManager.Authenticate(account);

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
    }
}
