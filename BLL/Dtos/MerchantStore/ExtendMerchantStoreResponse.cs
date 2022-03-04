using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using BLL.Dtos.Apartment;
using BLL.Dtos.Menu;
using BLL.Dtos.Resident;

namespace BLL.Dtos.MerchantStore
{
    [Serializable]
    public class ExtendMerchantStoreResponse : MerchantStoreResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<MenuResponse> Menus { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public MerchantStoreUpdateRequest UpdatedMerchantStore { get; set; }
    }
}