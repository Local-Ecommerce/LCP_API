using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblMerchant
    {
        public TblMerchant()
        {
            TblCollections = new HashSet<TblCollection>();
            TblMenus = new HashSet<TblMenu>();
            TblMerchantStores = new HashSet<TblMerchantStore>();
            TblProductCategories = new HashSet<TblProductCategory>();
        }

        public int MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public int? AccountId { get; set; }
        public int? LevelId { get; set; }

        public virtual TblAccount Account { get; set; }
        public virtual TblMerchantLevel Level { get; set; }
        public virtual ICollection<TblCollection> TblCollections { get; set; }
        public virtual ICollection<TblMenu> TblMenus { get; set; }
        public virtual ICollection<TblMerchantStore> TblMerchantStores { get; set; }
        public virtual ICollection<TblProductCategory> TblProductCategories { get; set; }
    }
}
