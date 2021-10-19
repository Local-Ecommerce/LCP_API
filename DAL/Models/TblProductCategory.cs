using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblProductCategory
    {
        public int ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ApproveStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? MerchantId { get; set; }
        public int? ProductId { get; set; }
        public int? SystemCategoryId { get; set; }

        public virtual TblMerchant Merchant { get; set; }
        public virtual TblProduct Product { get; set; }
        public virtual TblSystemCategory SystemCategory { get; set; }
    }
}
