using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Account
{
    [Serializable]
    public class ExtendAccountResponse : AccountResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ResidentId { get; set; }
    }
}
