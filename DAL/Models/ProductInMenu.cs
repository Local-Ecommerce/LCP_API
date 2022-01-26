using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class ProductInMenu
    {
        public ProductInMenu()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string ProductInMenuId { get; set; }
        public string ProductName { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        public double? UnitCost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string ProductId { get; set; }
        public string ProductCombinationId { get; set; }
        public string MenuId { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductCombination ProductCombination { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
