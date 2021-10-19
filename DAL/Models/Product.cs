using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Product
    {
        public Product()
        {
            CollectionMappings = new HashSet<CollectionMapping>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductCategories = new HashSet<ProductCategory>();
            ProductCombinationBaseProducts = new HashSet<ProductCombination>();
            ProductCombinationProducts = new HashSet<ProductCombination>();
            ProductInMenus = new HashSet<ProductInMenu>();
        }

        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public double? DefaultPrice { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double? Weight { get; set; }

        public virtual ICollection<CollectionMapping> CollectionMappings { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<ProductCombination> ProductCombinationBaseProducts { get; set; }
        public virtual ICollection<ProductCombination> ProductCombinationProducts { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
    }
}
