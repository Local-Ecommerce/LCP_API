using System;

namespace BLL.Dtos.ProductCategory
{
    public class ProductCategoryResponse
    {
        public string ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ApproveStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MerchantId { get; set; }
        public string ProductId { get; set; }
        public string SystemCategoryId { get; set; }
    }
}
