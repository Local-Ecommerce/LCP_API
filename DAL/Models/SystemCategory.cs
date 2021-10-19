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
        public string UpdatedBy { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
