using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class SystemCategory
    {
        public SystemCategory()
        {
            InverseBelongToNavigation = new HashSet<SystemCategory>();
            Products = new HashSet<Product>();
        }

        public string SystemCategoryId { get; set; }
        public string SysCategoryName { get; set; }
        public int? Status { get; set; }
        public int? CategoryLevel { get; set; }
        public string BelongTo { get; set; }

        public virtual SystemCategory BelongToNavigation { get; set; }
        public virtual ICollection<SystemCategory> InverseBelongToNavigation { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
