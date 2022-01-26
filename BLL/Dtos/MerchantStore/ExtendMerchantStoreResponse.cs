using System.Collections.Generic;
using System.Text.Json.Serialization;
using BLL.Dtos.Apartment;
using BLL.Dtos.Resident;
using BLL.Dtos.StoreMenuDetail;

namespace BLL.Dtos.MerchantStore
{
    public class ExtendMerchantStoreResponse : MerchantStoreResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<StoreMenuDetailResponse> StoreMenuDetails { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public MerchantStoreUpdateRequest UpdatedMerchantStore { get; set; }
    }
}