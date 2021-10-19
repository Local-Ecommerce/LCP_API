﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Customer
    {
        public Customer()
        {
            DeliveryAddresses = new HashSet<DeliveryAddress>();
            Orders = new HashSet<Order>();
        }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlock { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AccountId { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}