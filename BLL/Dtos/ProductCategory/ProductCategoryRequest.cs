namespace BLL.Dtos.ProductCategory
{
    public class ProductCategoryRequest
    {
        public string CategoryName { get; set; }
        public string MerchantId { get; set; }
        public string ProductId { get; set; }
        public string SystemCategoryId { get; set; }
    }
}
