using System;

namespace BLL.Dtos.ProductCategory
{
    public class ProductCategoryResponse
    {
        public string ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ResidentId { get; set; }
        public string ProductId { get; set; }
        public string SystemCategoryId { get; set; }
    }
}
