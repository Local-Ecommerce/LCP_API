using System;

namespace BLL.Dtos.OrderDetail
{
    public class OrderDetailResponse
    {
        public string OrderDetailId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? FinalAmount { get; set; }
        public double? Discount { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitCost { get; set; }
        public int? Status { get; set; }
        public string OrderId { get; set; }
        public string ProductInMenuId { get; set; }
    }
}
