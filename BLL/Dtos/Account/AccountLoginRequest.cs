namespace BLL.Dtos.Account
{
    public class AccountLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public enum AccountStatus
    {
        ERROR = -1,
        SUCCESS = 0,
        ACCOUNT_NOT_FOUND = 1,
        UNAUTHORIZED_ACCOUNT = 2,
        ACCOUNT_ALREADY_EXISTS = 3,
        INVALID_USERNAME_PASSWORD = 4,
        INVALID_CONFIRM_PASSWORD = 5,
        DELETED_ACCOUNT = 6,
    }
}
