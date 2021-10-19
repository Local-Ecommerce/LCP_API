using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblCollectionMappings = new HashSet<TblCollectionMapping>();
            TblOrderDetails = new HashSet<TblOrderDetail>();
            TblProductCategories = new HashSet<TblProductCategory>();
            TblProductCombinationBaseProducts = new HashSet<TblProductCombination>();
            TblProductCombinationProducts = new HashSet<TblProductCombination>();
            TblProductInMenus = new HashSet<TblProductInMenu>();
        }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public double? DefaultPrice { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double? Weight { get; set; }

        public virtual ICollection<TblCollectionMapping> TblCollectionMappings { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual ICollection<TblProductCategory> TblProductCategories { get; set; }
        public virtual ICollection<TblProductCombination> TblProductCombinationBaseProducts { get; set; }
        public virtual ICollection<TblProductCombination> TblProductCombinationProducts { get; set; }
        public virtual ICollection<TblProductInMenu> TblProductInMenus { get; set; }
    }
}
