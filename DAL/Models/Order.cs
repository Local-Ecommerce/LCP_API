using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        public string OrderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double? TotalAmount { get; set; }
        public int? OrderStatus { get; set; }
        public double? Discount { get; set; }
        public string CustomerId { get; set; }
        public string MerchantStoreId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual MerchantStore MerchantStore { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
