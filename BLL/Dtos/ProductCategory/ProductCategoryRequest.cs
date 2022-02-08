using System;

namespace BLL.Dtos.ProductCategory
{
    [Serializable]
    public class ProductCategoryRequest
    {
        public string CategoryName { get; set; }
        public string ResidentId { get; set; }
        public string ProductId { get; set; }
        public string SystemCategoryId { get; set; }
    }
}
