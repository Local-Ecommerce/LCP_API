using BLL.Dtos.Apartment;
using BLL.Dtos.Resident;
using System.Text.Json.Serialization;

namespace BLL.Dtos.News
{
    public class ExtendNewsResponse : NewsResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }
    }
}
