using BLL.Dtos.ProductInMenu;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class ExtendMenuResponse : MenuResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ProductInMenuResponse> ProductInMenus { get; set; }
    }
}
