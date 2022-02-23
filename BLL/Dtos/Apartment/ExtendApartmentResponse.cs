using BLL.Dtos.Resident;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Apartment
{
    [Serializable]
    public class ExtendApartmentResponse : ApartmentResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ResidentResponse> Residents { get; set; }
    }
}
