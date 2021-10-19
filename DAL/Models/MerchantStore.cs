using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class MerchantStore
    {
        public MerchantStore()
        {
            Orders = new HashSet<Order>();
            StoreMenuDetails = new HashSet<StoreMenuDetail>();
        }

        public string MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlock { get; set; }
        public string MerchantId { get; set; }
        public string LocalZoneId { get; set; }

        public virtual LocalZone LocalZone { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<StoreMenuDetail> StoreMenuDetails { get; set; }
    }
}
