using BLL.Dtos.Apartment;
using BLL.Dtos.Merchant;
using BLL.Dtos.StoreMenuDetail;
using System;
using System.Collections.Generic;

namespace BLL.Dtos.MerchantStore
{
    public class MerchantStoreResponse
    {
        public string MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string MerchantId { get; set; }
        public string ApartmentId { get; set; }
        public MerchantResponse Merchant { get; set; }
        public ApartmentResponse Apartment { get; set; }
        public List<StoreMenuDetailResponse> StoreMenuDetails { get; set; }
    }
}
