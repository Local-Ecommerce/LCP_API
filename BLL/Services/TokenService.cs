using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BLL.Dtos.RefreshToken;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public TokenService(string key, TokenValidationParameters tokenValidationParameters)
        {
            _key = key;
            _tokenValidationParameters = tokenValidationParameters;
        }


        /// <summary>
        /// Generate Access Token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public string GenerateAccessToken(string id, string roleName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role, roleName),
                }),
                Expires = ExpiryTimeAccessToken(roleName),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
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
                    expiryTime = DateTime.Now.AddHours((double)TimeUnit.ONE_HOUR);
                    break;
                case ResidentType.MARKET_MANAGER:
                    expiryTime = DateTime.Now.AddHours((double)TimeUnit.ONE_HOUR);
                    break;
                case ResidentType.MERCHANT:
                    expiryTime = DateTime.Now.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
                    break;
                case ResidentType.CUSTOMER:
                    expiryTime = DateTime.Now.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
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
                    expiryTime = DateTime.Now.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
                    break;
                case ResidentType.MARKET_MANAGER:
                    expiryTime = DateTime.Now.AddHours((double)TimeUnit.TWENTY_FOUR_HOUR);
                    break;
                case ResidentType.MERCHANT:
                    expiryTime = DateTime.Now.AddYears(1);
                    break;
                case ResidentType.CUSTOMER:
                    expiryTime = DateTime.Now.AddYears(1);
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
        /// <returns></returns>
        public RefreshToken GenerateRefreshToken(string id, string randomString, string roleName)
        {
            return new RefreshToken
            {
                Token = randomString,
                AccessToken = GenerateAccessToken(id, roleName),
                AccountId = id,
                IsRevoked = false,
                CreatedDate = DateTime.Now,
                ExpiredDate = ExpiryTimeRefreshToken(roleName)
            };
        }


        /// <summary>
        /// Verify And Generate Token
        /// </summary>
        /// <param name="tokenDto"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public string VerifyAndGenerateToken(RefreshTokenDto tokenDto, RefreshToken refreshToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Validation 1 - Validation JWT token format
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
            var localExpiryDate = long.Parse(tokenInVerification.Claims
                                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(localExpiryDate);

            if (expiryDate > DateTime.UtcNow)
                return null;

            // Validation 4 - validate if revoked
            if (refreshToken.IsRevoked is true)
                return null;

            // Validation 5 - validate the id
            var id = tokenInVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

            if (refreshToken.AccountId != id)
                return null;

            // Validation 6 - validate stored token expiry date
            if (refreshToken.ExpiredDate < DateTime.Now)
                return null;

            // update current token
            var role = tokenInVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            string accessToken = GenerateAccessToken(id, role);
            refreshToken.AccessToken = accessToken;

            return accessToken;
        }


        /// <summary>
        /// Unix Time Stamp To Date Time
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
    }
}