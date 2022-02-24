using System;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Generate Access Token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        string GenerateAccessToken(string id, string roleName);


        /// <summary>
        /// Expiry Time Access Token
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        DateTime ExpiryTimeAccessToken(string roleName);

        /// <summary>
        /// Expiry Time Refresh Token
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        DateTime ExpiryTimeRefreshToken(string roleName);


        /// <summary>
        /// Generate Refresh Token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="randomString"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        RefreshToken GenerateRefreshToken(string id, string randomString, string roleName);

    }
}