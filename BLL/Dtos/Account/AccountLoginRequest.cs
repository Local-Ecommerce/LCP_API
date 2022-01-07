namespace BLL.Dtos.Account
{
    public class AccountLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
