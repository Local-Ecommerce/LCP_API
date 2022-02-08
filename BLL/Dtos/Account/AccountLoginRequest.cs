using System;

namespace BLL.Dtos.Account
{
    [Serializable]
    public class AccountLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
