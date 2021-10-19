using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblDeliveryAddress
    {
        public int DeliveryAddressId { get; set; }
        public string DeliveryAddress { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public bool? IsPrimaryAddress { get; set; }
        public int? CustomerId { get; set; }

        public virtual TblCustomer Customer { get; set; }
    }
}
