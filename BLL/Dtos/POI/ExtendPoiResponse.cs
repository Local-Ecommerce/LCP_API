using BLL.Dtos.Apartment;
using BLL.Dtos.Resident;
using System.Text.Json.Serialization;

namespace BLL.Dtos.POI
{
    public class ExtendPoiResponse : PoiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }
    }
}
