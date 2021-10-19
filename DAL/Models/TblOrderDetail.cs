using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblOrderDetail
    {
        public int OrderDetailId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? FinalAmount { get; set; }
        public double? Discount { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitCost { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }

        public virtual TblOrder Order { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
