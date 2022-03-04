using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class MerchantStore
    {
        public MerchantStore()
        {
            Menus = new HashSet<Menu>();
            Orders = new HashSet<Order>();
        }

        public string MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }

        public virtual Apartment Apartment { get; set; }
        public virtual Resident Resident { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
