using BLL.Dtos.Menu;
using System.Text.Json.Serialization;

namespace BLL.Dtos.StoreMenuDetail
{
    public class ExtendStoreMenuDetailResponse : StoreMenuDetailResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public MenuResponse Menu { get; set; }
    }
}
