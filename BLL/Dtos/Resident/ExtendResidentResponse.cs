using BLL.Dtos.Apartment;
using BLL.Dtos.Menu;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Resident
{
    public class ExtendResidentResponse : ResidentResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<MenuResponse> Menus { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ApartmentResponse Apartment { get; set; }
    }
}
