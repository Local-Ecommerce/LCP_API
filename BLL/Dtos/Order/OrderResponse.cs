using BLL.Dtos.OrderDetail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BLL.Dtos.Order
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double? TotalAmount { get; set; }
        public int? Status { get; set; }
        public double? Discount { get; set; }
        public string ResidentId { get; set; }
        public string MerchantStoreId { get; set; }
        public Collection<OrderDetailResponse> OrderDetails { get; set; }
    }
}
