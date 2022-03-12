using BLL.Dtos.Product;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ProductInMenu
{
    [Serializable]
    public class ExtendProductInMenuResponse : ProductInMenuResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ProductResponse Product { get; set; }
    }
}
