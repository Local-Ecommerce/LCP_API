using BLL.Dtos.Resident;
using BLL.Dtos.SystemCategory;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ProductCategory
{
    [Serializable]
    public class ExtendProductCategoryResponse : ProductCategoryResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SystemCategoryResponse SystemCategory { get; set; }
    }
}
