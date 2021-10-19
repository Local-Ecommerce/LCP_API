using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblPayment
    {
        public int PaymentId { get; set; }
        public double? PaymentAmount { get; set; }
        public DateTime? DateTime { get; set; }
        public int? OrderId { get; set; }
        public int? PaymentMethodId { get; set; }

        public virtual TblOrder Order { get; set; }
        public virtual TblPaymentMethod PaymentMethod { get; set; }
    }
}
