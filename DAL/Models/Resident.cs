using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Resident
    {
        public Resident()
        {
            Orders = new HashSet<Order>();
        }

        public string ResidentId { get; set; }
        public string CustomerId { get; set; }
        public string ApartmentId { get; set; }

        public virtual Apartment Apartment { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
