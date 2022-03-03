using System;

namespace BLL.Dtos.StoreMenuDetail
{
    [Serializable]
    public class StoreMenuDetailResponse
    {
        public string StoreMenuDetailId { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int? Status { get; set; }
        public string RepeatDate { get; set; }
        public string MenuId { get; set; }
        public string MerchantStoreId { get; set; }
    }
}
