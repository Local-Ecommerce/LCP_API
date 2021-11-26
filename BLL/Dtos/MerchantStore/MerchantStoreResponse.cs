using System;

namespace BLL.Dtos.MerchantStore
{
    public class MerchantStoreResponse
    {
        public string MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlock { get; set; }
        public string MerchantId { get; set; }
        public string LocalZoneId { get; set; }
    }
}
