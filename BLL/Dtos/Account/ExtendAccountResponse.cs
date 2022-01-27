using System.Text.Json.Serialization;

namespace BLL.Dtos.Account
{
    public class ExtendAccountResponse : AccountResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ResidentId { get; set; }
    }
}
