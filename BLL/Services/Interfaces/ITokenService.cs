using System;
using BLL.Dtos.RefreshToken;
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
        /// <param name="expiredDate"></param>
        /// <returns></returns>
        string GenerateAccessToken(string id, string roleName, out DateTime expiredDate);


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
        /// <param name="roleName"></param>
        /// <param name="expiredDate"></param>
        /// <returns></returns>
        RefreshToken GenerateRefreshToken(string id, string randomString, string roleName, out DateTime expiredDate);


        /// <summary>
        /// Verify And Generate Token
        /// </summary>
        /// <param name="tokenDto"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expiredDate"></param>
        /// <returns></returns>
        string VerifyAndGenerateToken(RefreshTokenDto tokenDto, RefreshToken refreshToken, out DateTime expiredDate);


        /// <summary>
        /// Unix Time Stamp To Date Time
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        DateTime UnixTimeStampToDateTime(long unixTimeStamp);


        /// <summary>
        /// Does Token Expired
        /// </summary>
        /// <param name="authorization"></param>
        void CheckTokenExpired(string authorization);
    }
}