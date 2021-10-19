using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblCustomer
    {
        public TblCustomer()
        {
            TblDeliveryAddresses = new HashSet<TblDeliveryAddress>();
            TblOrders = new HashSet<TblOrder>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool? IsDeleted { get; set; }
        public int? AccountId { get; set; }

        public virtual TblAccount Account { get; set; }
        public virtual ICollection<TblDeliveryAddress> TblDeliveryAddresses { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
