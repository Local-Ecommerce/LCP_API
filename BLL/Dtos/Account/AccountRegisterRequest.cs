using Microsoft.AspNetCore.Http;

namespace BLL.Dtos.Account
{
    public class AccountRegisterRequest : AccountImageForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class AccountImageForm
    {
        public IFormFile AvatarImage { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
