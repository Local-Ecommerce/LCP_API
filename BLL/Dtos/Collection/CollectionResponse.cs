using BLL.Dtos.Resident;
using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Collection
{
    public class CollectionResponse
    {
        public string CollectionId { get; set; }
        public string CollectionName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResidentResponse Resident { get; set; }
    }
}
