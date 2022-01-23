using System;

namespace BLL.Dtos.StoreMenuDetail
{
    public class StoreMenuDetailResponse
    {
        public string StoreMenuDetailId { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public int? Status { get; set; }
        public string MenuId { get; set; }
        public string MerchantStoreId { get; set; }
    }
}
