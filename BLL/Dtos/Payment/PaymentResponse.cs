using System;

namespace BLL.Dtos.Payment
{
    public class PaymentResponse
    {
        public string PaymentId { get; set; }
        public double? PaymentAmount { get; set; }
        public DateTime? DateTime { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethodId { get; set; }
        public int? Status { get; set; }
    }
}
