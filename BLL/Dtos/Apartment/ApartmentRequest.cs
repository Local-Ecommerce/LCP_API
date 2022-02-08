using System;

namespace BLL.Dtos.Apartment
{
    [Serializable]
    public class ApartmentRequest
    {
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
    }
}
