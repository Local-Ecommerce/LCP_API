using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblLocalZone
    {
        public TblLocalZone()
        {
            TblMerchantStores = new HashSet<TblMerchantStore>();
        }

        public int LocalZoneId { get; set; }
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TblMerchantStore> TblMerchantStores { get; set; }
    }
}
