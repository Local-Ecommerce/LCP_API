using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblProductCombination
    {
        public int BaseProductId { get; set; }
        public int ProductId { get; set; }
        public int? DefaultMinMax { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblProduct BaseProduct { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
