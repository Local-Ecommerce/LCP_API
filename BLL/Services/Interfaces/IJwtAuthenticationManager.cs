using System;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="roleName"></param>
        /// <param name="expiredDate"></param>
        /// <returns></returns>
        string Authenticate(string residentId, string roleName, DateTime expiredDate);
    }
}
