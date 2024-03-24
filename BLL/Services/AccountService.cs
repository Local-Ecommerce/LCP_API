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

namespace BLL.Services {
	public class AccountService(IUnitOfWork unitOfWork,
				ILogger logger,
				IMapper mapper,
				IRedisService redisService,
				IFirebaseService firebaseService,
				IUtilService utilService,
				ITokenService tokenService,
				IResidentService residentService) : IAccountService {

		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly ILogger _logger = logger;
		private readonly IMapper _mapper = mapper;
		private readonly IRedisService _redisService = redisService;
		private readonly IFirebaseService _firebaseService = firebaseService;
		private readonly IUtilService _utilService = utilService;
		private readonly IResidentService _residentService = residentService;
		private readonly ITokenService _tokenService = tokenService;

		/// <summary>
		/// Create Account
		/// </summary>
		/// <param name="account"></param>
		/// <param name="uid"></param>
		/// <returns></returns>
		public async Task<Account> Register(Account account, string uid) {
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
		public async Task DeleteAccount(string id) {
			//validate id
			Account account;
			try {
				account = await _unitOfWork.Accounts.FindAsync(a => a.AccountId.Equals(id));
			}
			catch (Exception e) {
				_logger.Error($"{nameof(DeleteAccount)}: {e.Message}");
				throw;
			}

			//delete account
			try {
				account.UpdatedDate = _utilService.CurrentTimeInVietnam();
				account.Status = (int)AccountStatus.DELETED_ACCOUNT;

				_unitOfWork.Accounts.Update(account);

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e) {
				_logger.Error($"{nameof(DeleteAccount)}: {e.Message}");
				throw;
			}
		}


		/// <summary>
		/// Get Account By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<ExtendAccountResponse> GetAccountById(string id) {
			ExtendAccountResponse extendAccountResponse;

			//get account from database
			try {
				Account account = await _unitOfWork.Accounts.FindAsync(acc => acc.AccountId.Equals(id));

				extendAccountResponse = _mapper.Map<ExtendAccountResponse>(account);
			}
			catch (Exception e) {
				_logger.Error($"{nameof(GetAccountById)}: {e.Message}");

				throw new EntityNotFoundException(typeof(Account), id);
			}

			return extendAccountResponse;
		}


		/// <summary>
		/// Login
		/// </summary>
		/// <param name="accountRequest"></param>
		/// <returns></returns>
		public async Task<ExtendAccountResponse> Login(AccountRequest accountRequest) {
			Account account;
			bool isCreate = false;
			DateTime accessTokenExpiredDate = DateTime.MinValue;
			Resident resident = null;

			//check valid user
			string uid = await _firebaseService.GetUIDByToken(accountRequest.FirebaseToken);

			try {
				//check if user exists
				account = await _unitOfWork.Accounts.GetAccountIncludeResidentAndRefreshToken(uid);

				//create new account if account is not existed yet
				if (account == null) {
					account = await Register(account, uid);
					isCreate = true;
				}

				RefreshToken refreshToken;

				//find resident role
				if (!account.RoleId.Equals(RoleId.ADMIN)) {
					List<Resident> residents = account.Residents.ToList();

					if (accountRequest.Role.Equals(ResidentType.CUSTOMER))
						resident = residents.Where(r => r.Type.Equals(ResidentType.CUSTOMER)).FirstOrDefault();
					else if (string.IsNullOrEmpty(accountRequest.Role)) {
						if (residents.Count == 2)
							resident = residents.Where(r => r.Type.Equals(ResidentType.MERCHANT)).FirstOrDefault();
						else {
							resident = residents.FirstOrDefault();
							if (resident.Type.Equals(ResidentType.CUSTOMER)) {
								resident = await _residentService.CreateMerchant(resident.ResidentId);
								residents.Add(resident);
								account.Residents = residents;
							}
						}
					}

					if (resident is null)
						throw new UnauthorizedAccessException($"Role {accountRequest.Role} is invalid.");

					refreshToken = _tokenService.GenerateRefreshToken(resident.ResidentId,
													_utilService.CreateId(""), resident.Type, out accessTokenExpiredDate);
				}
				else {
					//admin
					refreshToken = _tokenService.GenerateRefreshToken(account.AccountId,
													_utilService.CreateId(""), account.RoleId, out accessTokenExpiredDate);
				}

				//generate token
				List<RefreshToken> refreshTokens = account.RefreshTokens.ToList();
				if (refreshTokens is null) refreshTokens = new();
				else
					//revoke old refresh token
					foreach (RefreshToken rt in refreshTokens) {
						if (account.RoleId.Equals(RoleId.ADMIN) || (resident != null && rt.Token.EndsWith(resident.Type)))
							rt.IsRevoked = true;
					}


				refreshTokens.Add(refreshToken);

				if (isCreate) {
					account.RefreshTokens = refreshTokens;
					_unitOfWork.Accounts.Add(account);
				}
				else {
					account.RefreshTokens = refreshTokens;
					_unitOfWork.Accounts.Update(account);
				}

				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e) {
				_logger.Error($"{nameof(Login)}: {e.Message}");

				throw new UnauthorizedAccessException();
			}

			account.RefreshTokens = account.RoleId.Equals(RoleId.ADMIN) ?
																	account.RefreshTokens
																			.Where(rt => rt.IsRevoked == false)
																			.OrderByDescending(rt => rt.CreatedDate)
																			.ToList() :
																	account.RefreshTokens
																			.Where(rt => rt.IsRevoked == false && rt.Token.EndsWith((resident.Type)))
																			.OrderByDescending(rt => rt.CreatedDate)
																			.ToList();

			account.Residents = new List<Resident>() { resident };

			//create response
			ExtendAccountResponse response = _mapper.Map<ExtendAccountResponse>(account);
			ExtendRefreshTokenDto refreshTokenResponse = response.RefreshTokens.FirstOrDefault();
			refreshTokenResponse.AccessTokenExpiredDate = accessTokenExpiredDate;

			response.RefreshTokens = new Collection<ExtendRefreshTokenDto>() { refreshTokenResponse };

			return response;
		}


		/// <summary>
		/// Refresh Token
		/// </summary>
		/// <param name="refreshTokenDto"></param>
		/// <returns></returns>
		public async Task<ExtendRefreshTokenDto> RefreshToken(RefreshTokenDto refreshTokenDto) {
			RefreshToken refreshToken;
			try {
				refreshToken = await _unitOfWork.RefreshTokens.FindAsync(rt => rt.Token.Equals(refreshTokenDto.Token));
			}
			catch (Exception e) {
				_logger.Error("[AccountService.RefreshToken()]: " + e.Message);
				throw new EntityNotFoundException();
			}

			string accessToken = _tokenService.VerifyAndGenerateToken(refreshTokenDto, refreshToken, out DateTime expiredDate);

			if (accessToken == null)
				throw new UnauthorizedAccessException();

			try {
				_unitOfWork.RefreshTokens.Update(refreshToken);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e) {
				_logger.Error($"{nameof(RefreshToken)}: {e.Message}");
				throw;
			}

			return new ExtendRefreshTokenDto {
				Token = refreshTokenDto.Token,
				AccessToken = accessToken,
				AccessTokenExpiredDate = expiredDate
			};
		}

		/// <summary>
		/// Logout
		/// </summary>
		/// <param name="accessToken"></param>
		/// <returns></returns>
		public async Task<string> Logout(string accessToken) {
			try {
				RefreshToken refreshToken = await _unitOfWork.RefreshTokens.FindAsync(rt => rt.AccessToken.Equals(accessToken));

				refreshToken.IsRevoked = true;

				_unitOfWork.RefreshTokens.Update(refreshToken);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e) {
				_logger.Error($"{nameof(Logout)}: {e.Message}");
				throw;
			}

			return null;
		}
	}
}
