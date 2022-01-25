using BLL.Dtos.Menu;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.StoreMenuDetail
{
    public class StoreMenuDetailResponse
    {
        public string StoreMenuDetailId { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public int? Status { get; set; }
        public string RepeatDate { get; set; }
        public string MenuId { get; set; }
        public string MerchantStoreId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public MenuForStoreResponse Menu { get; set; }
    }
}
