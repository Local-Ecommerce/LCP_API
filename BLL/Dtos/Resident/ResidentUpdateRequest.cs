﻿using System;

namespace BLL.Dtos.Resident
{
    [Serializable]
    public class ResidentUpdateRequest
    {
        public string ResidentName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string DeliveryAddress { get; set; }
        public string ProfileImage { get; set; }
    }
}
