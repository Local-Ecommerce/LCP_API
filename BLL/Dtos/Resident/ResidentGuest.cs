using System;

namespace BLL.Dtos.Resident
{
    [Serializable]
    public class ResidentGuest
    {
        public string ResidentName { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
    }
}