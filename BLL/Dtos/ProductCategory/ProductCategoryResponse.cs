using BLL.Dtos.Resident;
using System;
using System.Text.Json.Serialization;

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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }
    }
}
