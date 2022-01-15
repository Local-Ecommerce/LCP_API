using BLL.Dtos.Apartment;
using BLL.Dtos.Resident;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.POI
{
    public class PoiResponse
    {
        public string PoiId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }
    }
}
