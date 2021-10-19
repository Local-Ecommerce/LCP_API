using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblPaymentMethod
    {
        public TblPaymentMethod()
        {
            TblPayments = new HashSet<TblPayment>();
        }

        public int PaymentMethodId { get; set; }
        public string PaymentName { get; set; }

        public virtual ICollection<TblPayment> TblPayments { get; set; }
    }
}
