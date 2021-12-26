using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class MarketManager
    {
        public MarketManager()
        {
            News = new HashSet<News>();
            Pois = new HashSet<Poi>();
        }

        public string MarketManagerId { get; set; }
        public string MarketManagerName { get; set; }
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public string AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Poi> Pois { get; set; }
    }
}
