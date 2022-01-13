using BLL.Dtos.Merchant;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Menu
{
    public class MenuResponse
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string MerchantId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public MerchantResponse Merchant { get; set; }
    }
}
