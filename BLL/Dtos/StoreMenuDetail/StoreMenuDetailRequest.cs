using System;

namespace BLL.Dtos.StoreMenuDetail
{
    public class StoreMenuDetailRequest
    {
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public int? Status { get; set; }
        public string RepeatDate { get; set; }
        public string MenuId { get; set; }
        public string MerchantStoreId { get; set; }
    }
}
