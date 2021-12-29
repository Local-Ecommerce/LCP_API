using System;

namespace BLL.Dtos.StoreMenuDetail
{
    public class StoreMenuDetailResponse
    {
        public string StoreMenuDetailId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string MenuId { get; set; }
        public string MerchantStoreId { get; set; }
    }
}
