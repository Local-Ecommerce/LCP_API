namespace BLL.Dtos.DeliveryAddress
{
    public class DeliveryAddressRequest
    {
        public string DeliveryAddress { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public string CustomerId { get; set; }
    }
}
