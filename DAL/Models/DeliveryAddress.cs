using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class DeliveryAddress
    {
        public string DeliveryAddressId { get; set; }
        public string DeliveryAddress1 { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public bool? IsPrimaryAddress { get; set; }
        public string CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
