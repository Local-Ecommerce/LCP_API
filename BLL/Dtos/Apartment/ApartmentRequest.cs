using System;

namespace BLL.Dtos.Apartment
{
    [Serializable]
    public class ApartmentRequest
    {
        public string ApartmentName { get; set; }
        public string Address { get; set; }
    }
}
