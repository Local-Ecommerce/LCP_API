using BLL.Dtos.ProductInMenu;
using BLL.Dtos.Resident;
using BLL.Dtos.StoreMenuDetail;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class ExtendMenuResponses : MenuResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }

        [DefaultValue(null)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ProductInMenuResponse> ProductInMenus { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<StoreMenuDetailResponse> StoreMenuDetails { get; set; }
    }
}
