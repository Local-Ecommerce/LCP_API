using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using BLL.Dtos.RefreshToken;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly byte[] _key;
        private TokenValidationParameters _tokenValidationParameters;
        public TokenService(byte[] key, TokenValidationParameters tokenValidationParameters)
        {
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
        public string GenerateAccessToken(string id, string roleName, out DateTime expiredDate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            expiredDate = ExpiryTimeAccessToken(roleName);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role, roleName),
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
        public DateTime ExpiryTimeAccessToken(string roleName)
        {
            DateTime expiryTime = DateTime.MinValue;
            switch (roleName)
            {
                case RoleId.ADMIN:
                    expiryTime = DateTime.UtcNow.AddHours((double)TimeUnit.ONE_HOUR);
                    break;
                case ResidentType.MARKET_MANAGER:
                    expiryTime = DateTime.UtcNow.AddHours((double)TimeUnit.ONE_HOUR);
                    break;
                case ResidentType.MERCHANT:
                    expiryTime = DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
                    break;
                case ResidentType.CUSTOMER:
                    expiryTime = DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
                    break;
            }
            return expiryTime;
        }


        /// <summary>
        /// Expiry Time Refresh Token
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public DateTime ExpiryTimeRefreshToken(string roleName)
        {
            DateTime expiryTime = DateTime.MinValue;
            switch (roleName)
            {
                case RoleId.ADMIN:
                    expiryTime = DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
                    break;
                case ResidentType.MARKET_MANAGER:
                    expiryTime = DateTime.UtcNow.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
                    break;
                case ResidentType.MERCHANT:
                    expiryTime = DateTime.UtcNow.AddYears(1);
                    break;
                case ResidentType.CUSTOMER:
                    expiryTime = DateTime.UtcNow.AddYears(1);
                    break;
            }
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
        public RefreshToken GenerateRefreshToken(string id, string randomString, string roleName, out DateTime expiredDate)
        {
            return new RefreshToken
            {
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
        public string VerifyAndGenerateToken(RefreshTokenDto tokenDto, RefreshToken refreshToken, out DateTime expiredDate)
        {
            _tokenValidationParameters.ValidateLifetime = false;
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            expiredDate = DateTime.MinValue;

            // Validation 1 - Validation JWT token format
            try
            {
                var tokenInVerification = jwtTokenHandler
            .ValidateToken(tokenDto.AccessToken, _tokenValidationParameters, out var validatedToken);


                // Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
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
                string accountId = residentId[..residentId.IndexOf("_")];

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
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Unix Time Stamp To Date Time
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }


        /// <summary>
        /// Does Token Expired
        /// </summary>
        /// <param name="authorization"></param>
        public void CheckTokenExpired(string authorization)
        {
            //get token
            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var scheme = headerValue.Scheme;
                var parameter = headerValue.Parameter;
            }

            _tokenValidationParameters.ValidateLifetime = false;
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenInVerification = jwtTokenHandler
                .ValidateToken(headerValue.Parameter, _tokenValidationParameters, out var validatedToken);

            var utcExpiryDate = long.Parse(tokenInVerification.Claims
                                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            if (UnixTimeStampToDateTime(utcExpiryDate) < DateTime.UtcNow)
            {
                throw new TimeoutException();
            }
        }
    }
};