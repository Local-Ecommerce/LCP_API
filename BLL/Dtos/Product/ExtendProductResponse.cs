using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class ExtendProductResponse : ProductResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ProductResponse> RelatedProducts { get; set; }
    }
}