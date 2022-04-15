using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Payment
    {
        public string PaymentId { get; set; }
        public double? PaymentAmount { get; set; }
        public DateTime? DateTime { get; set; }
        public long? TransactionId { get; set; }
        public int? ResultCode { get; set; }
        public int? Status { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethodId { get; set; }

        public virtual Order Order { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
