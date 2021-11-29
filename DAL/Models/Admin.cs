using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Admin
    {
        public string AdminId { get; set; }
        public string AdminName { get; set; }
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public string AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
