using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class ExtendProductResponse : UpdateProductResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<UpdateProductResponse> RelatedProducts { get; set; }
    }


    [Serializable]
    public class UpdateProductResponse : ProductResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ProductResponse CurrentProduct { get; set; }
    }
}