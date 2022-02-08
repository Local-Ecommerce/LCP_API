using BLL.Dtos.Product;
using BLL.Dtos.ProductCombination;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ProductInMenu
{
    [Serializable]
    public class ExtendProductInMenuResponse : ProductInMenuResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ProductResponse Product { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ProductCombinationResponse ProductCombination { get; set; }
    }
}
