using System;

namespace BLL.Dtos.Apartment
{
    [Serializable]
    public class ApartmentResponse
    {
        public string ApartmentId { get; set; }
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public int? Status { get; set; }
    }
}
