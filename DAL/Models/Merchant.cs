using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Merchant
    {
        public Merchant()
        {
            Collections = new HashSet<Collection>();
            Menus = new HashSet<Menu>();
            MerchantStores = new HashSet<MerchantStore>();
            ProductCategories = new HashSet<ProductCategory>();
        }

        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public string ApproveBy { get; set; }
        public string AccountId { get; set; }
        public string LevelId { get; set; }

        public virtual Account Account { get; set; }
        public virtual MerchantLevel Level { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<MerchantStore> MerchantStores { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
