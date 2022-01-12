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
        public string DeliveryAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double? TotalAmount { get; set; }
        public int? Status { get; set; }
        public double? Discount { get; set; }
        public string ResidentId { get; set; }
        public string MerchantStoreId { get; set; }

        public virtual MerchantStore MerchantStore { get; set; }
        public virtual Resident Resident { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
