using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class ProductCombination
    {
        public string BaseProductId { get; set; }
        public string ProductId { get; set; }
        public int? DefaultMinMax { get; set; }
        public int? Status { get; set; }

        public virtual Product BaseProduct { get; set; }
        public virtual Product Product { get; set; }
    }
}
