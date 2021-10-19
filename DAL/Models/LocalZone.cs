using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class LocalZone
    {
        public LocalZone()
        {
            MerchantStores = new HashSet<MerchantStore>();
        }

        public string LocalZoneId { get; set; }
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<MerchantStore> MerchantStores { get; set; }
    }
}
