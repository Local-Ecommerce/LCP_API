using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblAccount
    {
        public TblAccount()
        {
            TblCustomers = new HashSet<TblCustomer>();
            TblMerchants = new HashSet<TblMerchant>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public string AvatarImage { get; set; }

        public virtual ICollection<TblCustomer> TblCustomers { get; set; }
        public virtual ICollection<TblMerchant> TblMerchants { get; set; }
    }
}
