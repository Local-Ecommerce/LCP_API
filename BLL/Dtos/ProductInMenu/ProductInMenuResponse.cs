using BLL.Dtos.Product;
using BLL.Dtos.ProductCombination;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ProductInMenu
{
    public class ProductInMenuResponse
    {
        public string ProductInMenuId { get; set; }
        public double? Price { get; set; }
        public double? UnitCost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string ProductId { get; set; }
        public string ProductCombinationId { get; set; }
        public string MenuId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ProductResponse Product { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ProductCombinationResponse ProductCombination { get; set; }
    }
}
