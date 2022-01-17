namespace BLL.Dtos.OrderDetail
{
    public class OrderDetailRequest
    {
        public int? Quantity { get; set; }
        public double? Discount { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitCost { get; set; }
        public int? Status { get; set; }
        public string MerchantStoreId { get; set; }
        public string ProductInMenuId { get; set; }
    }
}
