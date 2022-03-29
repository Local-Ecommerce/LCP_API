using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Menu
    {
        public Menu()
        {
            ProductInMenus = new HashSet<ProductInMenu>();
        }

        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string RepeatDate { get; set; }
        public int? Status { get; set; }
        public bool? BaseMenu { get; set; }
        public bool? IncludeBaseMenu { get; set; }
        public string MerchantStoreId { get; set; }

        public virtual MerchantStore MerchantStore { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
    }
}
