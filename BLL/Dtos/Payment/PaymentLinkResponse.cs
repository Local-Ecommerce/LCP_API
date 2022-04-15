using System;

namespace BLL.Dtos.Payment
{
    [Serializable]
    public class PaymentLinkResponse
    {
        public string PayUrl { get; set; }
        public string Deeplink { get; set; }
    }
}