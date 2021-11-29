using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountTokens = new HashSet<AccountToken>();
            Admins = new HashSet<Admin>();
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
        public int? Status { get; set; }

        public virtual ICollection<AccountToken> AccountTokens { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<MarketManager> MarketManagers { get; set; }
        public virtual ICollection<Merchant> Merchants { get; set; }
    }
}
