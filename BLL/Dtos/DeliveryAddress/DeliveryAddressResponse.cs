namespace BLL.Dtos.DeliveryAddress
{
    public class DeliveryAddressResponse
    {
        public string DeliveryAddressId { get; set; }
        public string DeliveryAddress1 { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public bool? IsPrimaryAddress { get; set; }
        public string CustomerId { get; set; }
    }
}
