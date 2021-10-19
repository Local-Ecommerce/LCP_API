using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Payments = new HashSet<Payment>();
        }

        public string PaymentMethodId { get; set; }
        public string PaymentName { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
