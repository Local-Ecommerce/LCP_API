using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Apartment
    {
        public Apartment()
        {
            MerchantStores = new HashSet<MerchantStore>();
            News = new HashSet<News>();
            Pois = new HashSet<Poi>();
        }

        public string ApartmentId { get; set; }
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<MerchantStore> MerchantStores { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Poi> Pois { get; set; }
    }
}
