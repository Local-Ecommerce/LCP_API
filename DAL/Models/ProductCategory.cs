using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class ProductCategory
    {
        public string ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MerchantId { get; set; }
        public string ProductId { get; set; }
        public string SystemCategoryId { get; set; }

        public virtual Merchant Merchant { get; set; }
        public virtual Product Product { get; set; }
        public virtual SystemCategory SystemCategory { get; set; }
    }
}
