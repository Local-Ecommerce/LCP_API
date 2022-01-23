using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class StoreMenuDetail
    {
        public string StoreMenuDetailId { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public int? Status { get; set; }
        public string RepeatDate { get; set; }
        public string MenuId { get; set; }
        public string MerchantStoreId { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual MerchantStore MerchantStore { get; set; }
    }
}
