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
            StoreMenuDetails = new HashSet<StoreMenuDetail>();
        }

        public string MenuId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string MerchantId { get; set; }

        public virtual Merchant Merchant { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
        public virtual ICollection<StoreMenuDetail> StoreMenuDetails { get; set; }
    }
}
