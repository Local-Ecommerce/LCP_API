namespace BLL.Dtos.Apartment
{
    public class ApartmentRequest
    {
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
    }
}
