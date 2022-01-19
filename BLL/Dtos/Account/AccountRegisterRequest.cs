﻿namespace BLL.Dtos.Account
{
    public class AccountRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AvatarImage { get; set; }
        public string ProfileImage { get; set; }

    }
}
