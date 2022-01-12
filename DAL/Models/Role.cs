using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
