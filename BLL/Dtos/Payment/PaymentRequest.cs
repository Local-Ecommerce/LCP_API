using System.Collections.Generic;

namespace BLL.Dtos.Payment
{
    public class PaymentRequest
    {
        public double? PaymentAmount { get; set; }
        public List<string> OrderIds { get; set; }
        public string PaymentMethodId { get; set; }
        public string RedirectUrl { get; set; }
    }
}
