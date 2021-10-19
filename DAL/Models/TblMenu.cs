using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblMenu
    {
        public TblMenu()
        {
            TblProductInMenus = new HashSet<TblProductInMenu>();
            TblStoreMenuDetails = new HashSet<TblStoreMenuDetail>();
        }

        public int MenuId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? MerchantId { get; set; }

        public virtual TblMerchant Merchant { get; set; }
        public virtual ICollection<TblProductInMenu> TblProductInMenus { get; set; }
        public virtual ICollection<TblStoreMenuDetail> TblStoreMenuDetails { get; set; }
    }
}
