using System;

namespace BLL.Dtos.JWT
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string ResidentId { get; set; }
    }
}
