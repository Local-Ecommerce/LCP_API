using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class AccountToken
    {
        public string TokenId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
