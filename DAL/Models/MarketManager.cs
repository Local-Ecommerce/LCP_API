using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class MarketManager
    {
        public string MarketManagerId { get; set; }
        public string MarketManagerName { get; set; }
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public string AccountId { get; set; }
        public string AparmentId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Apartment Aparment { get; set; }
    }
}
