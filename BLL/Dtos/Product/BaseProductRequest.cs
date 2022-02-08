using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class BaseProductRequest : ProductRequest
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ProductRequest> InverseBelongToNavigation { get; set; }
    }
}