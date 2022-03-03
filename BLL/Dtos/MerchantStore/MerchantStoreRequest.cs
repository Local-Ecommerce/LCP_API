using System;

namespace BLL.Dtos.MerchantStore
{
    [Serializable]
    public class MerchantStoreRequest
    {
        public string StoreName { get; set; }
        public string ApartmentId { get; set; }
    }
}
