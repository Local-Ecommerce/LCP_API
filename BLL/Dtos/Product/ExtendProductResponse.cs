using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Product
{
    public class ExtendProductResponse : ProductResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ProductResponse> InverseBelongToNavigation { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UpdateProductRequest UpdatedProduct { get; set; }
    }
}