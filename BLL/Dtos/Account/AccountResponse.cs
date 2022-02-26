using System;

namespace BLL.Dtos.Account
{
    [Serializable]
    public class AccountResponse
    {
        public string AccountId { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public string AvatarImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string RoleId { get; set; }
    }
}
