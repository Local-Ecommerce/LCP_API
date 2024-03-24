using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using BLL.Dtos.RefreshToken;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services {
	public class TokenService : ITokenService {
		private readonly byte[] _key;
		private readonly TokenValidationParameters _tokenValidationParameters;
		public TokenService(byte[] key, TokenValidationParameters tokenValidationParameters) {
			_key = key;
			_tokenValidationParameters = tokenValidationParameters;
		}

		/// <summary>
		/// Generate Access Token
		/// </summary>
		/// <param name="id"></param>
		/// <param name="roleName"></param>
		/// <param name="expiredDate"></param>
		/// <returns></returns>
		public string GenerateAccessToken(string id, string roleName, out DateTime expiredDate) {
			var tokenHandler = new JwtSecurityTokenHandler();
			expiredDate = ExpiryTimeAccessToken(roleName);

			var tokenDescriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(new Claim[] {
						new(ClaimTypes.Name, id),
						new(ClaimTypes.Role, roleName),
				}),
				Expires = expiredDate,

				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(_key),
					SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		/// <summary>
		/// Expiry Time Access Token
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public DateTime ExpiryTimeAccessToken(string roleName) {
			var expiryTime = roleName switch {
				RoleId.ADMIN => DateTime.UtcNow.AddHours((double)TimeUnit.ONE_HOUR),
				ResidentType.MARKET_MANAGER => DateTime.UtcNow.AddHours((double)TimeUnit.ONE_HOUR),
				ResidentType.MERCHANT => DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR),
				ResidentType.CUSTOMER => DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR),
				_ => DateTime.MinValue
			};

			return expiryTime;
		}

		/// <summary>
		/// Expiry Time Refresh Token
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		public DateTime ExpiryTimeRefreshToken(string roleName) {
			var expiryTime = roleName switch {
				RoleId.ADMIN => DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR),
				ResidentType.MARKET_MANAGER => DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR),
				ResidentType.MERCHANT => DateTime.UtcNow.AddYears(1),
				ResidentType.CUSTOMER => DateTime.UtcNow.AddYears(1),
				_ => DateTime.MinValue
			};

			return expiryTime;
		}


		/// <summary>
		/// Generate Refresh Token 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="randomString"></param>
		/// <param name="accessToken"></param>
		/// <param name="roleName"></param>
		/// <param name="expiredDate"></param>
		/// <returns></returns>
		public RefreshToken GenerateRefreshToken(string id, string randomString, string roleName, out DateTime expiredDate) {
			return new RefreshToken {
				Token = randomString + "_" + roleName,
				AccessToken = GenerateAccessToken(id, roleName, out expiredDate),
				AccountId = id,
				IsRevoked = false,
				CreatedDate = DateTime.UtcNow,
				ExpiredDate = ExpiryTimeRefreshToken(roleName)
			};
		}


		/// <summary>
		/// Verify And Generate Token
		/// </summary>
		/// <param name="tokenDto"></param>
		/// <param name="refreshToken"></param>
		/// <param name="expiredDate"></param>
		/// <returns></returns>
		public string VerifyAndGenerateToken(RefreshTokenDto tokenDto, RefreshToken refreshToken, out DateTime expiredDate) {
			_tokenValidationParameters.ValidateLifetime = false;
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			expiredDate = DateTime.MinValue;

			// Validation 1 - Validation JWT token format
			try {
				var tokenInVerification = jwtTokenHandler
					.ValidateToken(tokenDto.AccessToken, _tokenValidationParameters, out var validatedToken);

				// Validation 2 - Validate encryption alg
				if (validatedToken is JwtSecurityToken jwtSecurityToken) {
					var result = jwtSecurityToken.Header.Alg
									.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

					if (result == false)
						return null;
				}

				// Validation 3 - validate expiry date
				var utcExpiryDate = long.Parse(tokenInVerification.Claims
																.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

				if (UnixTimeStampToDateTime(utcExpiryDate) > DateTime.UtcNow)
					return null;

				// Validation 4 - validate if revoked
				if (refreshToken.IsRevoked is true)
					return null;

				// Validation 5 - validate the id
				var residentId = tokenInVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
				string accountId = residentId.Contains("_") ? residentId[..residentId.IndexOf("_")] : residentId;

				if (refreshToken.AccountId != accountId)
					return null;

				// Validation 6 - validate stored token expiry date
				if (refreshToken.ExpiredDate < DateTime.UtcNow)
					return null;

				// update current token
				var role = tokenInVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
				string accessToken = GenerateAccessToken(accountId, role, out expiredDate);
				refreshToken.AccessToken = accessToken;

				return accessToken;

			}
			catch (Exception) {
				throw;
			}
		}

		/// <summary>
		/// Unix Time Stamp To Date Time
		/// </summary>
		/// <param name="unixTimeStamp"></param>
		/// <returns></returns>
		public DateTime UnixTimeStampToDateTime(long unixTimeStamp) {
			var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

			return dateTimeVal;
		}

		/// <summary>
		/// Does Token Expired
		/// </summary>
		/// <param name="authorization"></param>
		public void CheckTokenExpired(string authorization) {
			//get token
			if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue)) {
				var scheme = headerValue.Scheme;
				var parameter = headerValue.Parameter;
			}

			_tokenValidationParameters.ValidateLifetime = false;
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var tokenInVerification = jwtTokenHandler
					.ValidateToken(headerValue.Parameter, _tokenValidationParameters, out var validatedToken);

			var utcExpiryDate = long.Parse(tokenInVerification.Claims
															.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

			if (UnixTimeStampToDateTime(utcExpiryDate) < DateTime.UtcNow) {
				throw new TimeoutException();
			}
		}

		/// <summary>
		/// Get Resident Id And Role
		/// </summary>
		/// <param name="identity"></param>
		/// <returns></returns>
		public (string, string) GetResidentIdAndRole(ClaimsIdentity identity) {
		IEnumerable<Claim> claim = identity.Claims;
			string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
			string residentId = claimName[(claimName.LastIndexOf(':') + 2)..];

			//get role from token
			string claimRole = claim.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().ToString();
			string role = claimRole[(claimRole.LastIndexOf(':') + 2)..];

			return (residentId, role);
		}
	}
};