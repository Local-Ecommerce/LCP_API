using System;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class MenuResponse
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string RepeatDate { get; set; }
        public bool? BaseMenu { get; set; }
        public bool? IncludeBaseMenu { get; set; }
        public string MerchantStoreId { get; set; }
    }
}
