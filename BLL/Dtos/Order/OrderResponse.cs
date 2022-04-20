using System;
namespace BLL.Dtos.Order
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double? TotalAmount { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string MerchantStoreId { get; set; }
    }
}
