using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class ProductCombination
    {
        public ProductCombination()
        {
            ProductInMenus = new HashSet<ProductInMenu>();
        }

        public string ProductCombinationId { get; set; }
        public string BaseProductId { get; set; }
        public string ProductId { get; set; }
        public int? DefaultMin { get; set; }
        public string DefaultMax { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? Status { get; set; }

        public virtual Product BaseProduct { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
    }
}
