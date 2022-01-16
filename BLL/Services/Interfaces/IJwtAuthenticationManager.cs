using System;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string residentId, string roleName, DateTime expiredDate);
    }
}
