namespace BLL.Dtos.Payment
{
    public class PaymentRequest
    {
        public double? PaymentAmount { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}
