using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class OrderDetail
    {
        public string OrderDetailId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? FinalAmount { get; set; }
        public double? UnitPrice { get; set; }
        public int? Status { get; set; }
        public string OrderId { get; set; }
        public string ProductInMenuId { get; set; }

        public virtual Order Order { get; set; }
        public virtual ProductInMenu ProductInMenu { get; set; }
    }
}
