using BLL.Constants;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string _key;
        public JwtAuthenticationManager(string key)
        {
            _key = key;
        }

        public string Authenticate(Account account, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            DateTime expires = role.Equals(Role.Admin) || role.Equals(Role.MarketManager)
                                ? DateTime.UtcNow.AddHours((double)TimeUnit.ONE_HOUR) 
                                : DateTime.UtcNow.AddDays((double)TimeUnit.THIRTY_DAYS);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.AccountId),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = expires,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
