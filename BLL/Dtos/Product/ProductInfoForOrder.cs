namespace BLL.Dtos.Product
{
    public class ProductInfoForOrder
    {
        public double Price { get; set; }
        public string MerchantStoreId { get; set; }
        public string ProductInMenuId { get; set; }
        public int Quantity { get; set; }
        public int MaxBuy { get; set; }
    }
}