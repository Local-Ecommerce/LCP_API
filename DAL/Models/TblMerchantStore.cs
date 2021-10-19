using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblMerchantStore
    {
        public TblMerchantStore()
        {
            TblOrders = new HashSet<TblOrder>();
            TblStoreMenuDetails = new HashSet<TblStoreMenuDetail>();
        }

        public int MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? MerchantId { get; set; }
        public int? LocalZoneId { get; set; }

        public virtual TblLocalZone LocalZone { get; set; }
        public virtual TblMerchant Merchant { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
        public virtual ICollection<TblStoreMenuDetail> TblStoreMenuDetails { get; set; }
    }
}
