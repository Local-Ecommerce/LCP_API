using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Account
    {
        public Account()
        {
            RefreshTokens = new HashSet<RefreshToken>();
            Residents = new HashSet<Resident>();
        }

        public string AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public string AvatarImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string RoleId { get; set; }
        public int? Status { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Resident> Residents { get; set; }
    }
}
