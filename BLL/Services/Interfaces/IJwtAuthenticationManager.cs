using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(Account account, string role);
    }
}
