using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Account
    {
        public Account()
        {
            Customers = new HashSet<Customer>();
            MarketManagers = new HashSet<MarketManager>();
            Merchants = new HashSet<Merchant>();
        }

        public string AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public string AvatarImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpiredDate { get; set; }
        public string RoleId { get; set; }
        public int? Status { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<MarketManager> MarketManagers { get; set; }
        public virtual ICollection<Merchant> Merchants { get; set; }
    }
}
