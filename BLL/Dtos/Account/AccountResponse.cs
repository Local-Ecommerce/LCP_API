using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Account
{
    public class AccountResponse
    {
        public string AccountId { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public string AvatarImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string Token { get; set; }
        public string TokenExpiredDate { get; set; }
        public string RoleId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ClientRoleId { get; set; }
    }
}
