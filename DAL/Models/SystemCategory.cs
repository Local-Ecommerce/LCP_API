using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class SystemCategory
    {
        public SystemCategory()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }

        public string SystemCategoryId { get; set; }
        public string SysCategoryName { get; set; }
        public string ApproveBy { get; set; }
        public int? Status { get; set; }
        public int? CategoryLevel { get; set; }
        public string BelongTo { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
