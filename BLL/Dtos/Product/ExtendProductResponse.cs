using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using BLL.Dtos.ProductCategory;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class ExtendProductResponse : ProductResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ProductResponse> InverseBelongToNavigation { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UpdateProductRequest UpdatedProduct { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ExtendProductCategoryResponse> ProductCategories { get; set; }
    }
}