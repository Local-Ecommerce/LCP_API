using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Apartment
    {
        public Apartment()
        {
            MarketManagers = new HashSet<MarketManager>();
            MerchantStores = new HashSet<MerchantStore>();
        }

        public string ApartmentId { get; set; }
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<MarketManager> MarketManagers { get; set; }
        public virtual ICollection<MerchantStore> MerchantStores { get; set; }
    }
}
