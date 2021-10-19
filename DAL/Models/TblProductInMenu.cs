using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblProductInMenu
    {
        public int ProductInMenuId { get; set; }
        public double? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? ProductId { get; set; }
        public int? MenuId { get; set; }

        public virtual TblMenu Menu { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
