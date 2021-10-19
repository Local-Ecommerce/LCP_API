using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblStoreMenuDetail
    {
        public int PriceMenuDetailId { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? MenuId { get; set; }
        public int? MerchantStoreId { get; set; }

        public virtual TblMenu Menu { get; set; }
        public virtual TblMerchantStore MerchantStore { get; set; }
    }
}
