using BLL.Dtos.Resident;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ProductCategory
{
    public class ExtendProductCategoryResponse : ProductCategoryResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }
    }
}
