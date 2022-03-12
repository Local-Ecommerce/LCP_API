using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Product
    {
        public Product()
        {
            InverseBelongToNavigation = new HashSet<Product>();
            ProductInMenus = new HashSet<ProductInMenu>();
        }

        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double? DefaultPrice { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ApproveBy { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double? Weight { get; set; }
        public int? IsFavorite { get; set; }
        public string BelongTo { get; set; }
        public string ResidentId { get; set; }
        public string SystemCategoryId { get; set; }

        public virtual Product BelongToNavigation { get; set; }
        public virtual Resident Resident { get; set; }
        public virtual SystemCategory SystemCategory { get; set; }
        public virtual ICollection<Product> InverseBelongToNavigation { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
    }
}
