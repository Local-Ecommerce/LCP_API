using BLL.Dtos.Menu;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.StoreMenuDetail
{
    [Serializable]
    public class ExtendStoreMenuDetailResponse : StoreMenuDetailResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public MenuResponse Menu { get; set; }
    }
}
