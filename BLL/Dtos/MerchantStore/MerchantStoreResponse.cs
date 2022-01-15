using BLL.Dtos.Apartment;
using BLL.Dtos.Resident;
using BLL.Dtos.StoreMenuDetail;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BLL.Dtos.MerchantStore
{
    public class MerchantStoreResponse
    {
        public string MerchantStoreId { get; set; }
        public string StoreName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<StoreMenuDetailResponse> StoreMenuDetails { get; set; }
    }
}
