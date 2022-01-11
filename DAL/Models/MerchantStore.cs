using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class MerchantStore
    {
        public MerchantStore()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Orders = new HashSet<Order>();
            StoreMenuDetails = new HashSet<StoreMenuDetail>();
        }

        public string MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string MerchantId { get; set; }
        public string ApartmentId { get; set; }

        public virtual Apartment Apartment { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<StoreMenuDetail> StoreMenuDetails { get; set; }
    }
}
