using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class StoreMenuDetail
    {
        public string StoreMenuDetailId { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string MenuId { get; set; }
        public string MerchantStoreId { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual MerchantStore MerchantStore { get; set; }
    }
}
