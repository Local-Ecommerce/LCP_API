using BLL.Dtos.Apartment;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Resident
{
    [Serializable]
    public class ExtendResidentResponse : ResidentResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }
    }
}
