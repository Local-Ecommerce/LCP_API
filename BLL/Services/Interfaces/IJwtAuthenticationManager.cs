using System;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string clientRoleId, string roleName, DateTime expiredDate);
    }
}
