using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        public TokenService(string key)
        {
            _key = key;
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
    }
}