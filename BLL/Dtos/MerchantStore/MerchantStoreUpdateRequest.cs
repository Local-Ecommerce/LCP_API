using System;

namespace BLL.Dtos.MerchantStore
{
    [Serializable]
    public class MerchantStoreUpdateRequest
    {
        public string StoreName { get; set; }
        public int? Status { get; set; }
        public string ApartmentId { get; set; }
    }
}