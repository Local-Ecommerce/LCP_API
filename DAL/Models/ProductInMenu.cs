﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class ProductInMenu
    {
        public ProductInMenu()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string ProductInMenuId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? MaxBuy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string ProductId { get; set; }
        public string MenuId { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
