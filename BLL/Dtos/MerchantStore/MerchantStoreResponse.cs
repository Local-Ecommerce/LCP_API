﻿using System;

namespace BLL.Dtos.MerchantStore
{
    [Serializable]
    public class MerchantStoreResponse
    {
        public string MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string StoreImage { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Warned { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }
    }
}
