using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
            TblPayments = new HashSet<TblPayment>();
        }

        public int OrderId { get; set; }
        public DateTime? BookedDate { get; set; }
        public double? TotalAmount { get; set; }
        public int? OrderStatus { get; set; }
        public double? Discount { get; set; }
        public int? CustomerId { get; set; }
        public int? MerchantStoreId { get; set; }

        public virtual TblCustomer Customer { get; set; }
        public virtual TblMerchantStore MerchantStore { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual ICollection<TblPayment> TblPayments { get; set; }
    }
}
