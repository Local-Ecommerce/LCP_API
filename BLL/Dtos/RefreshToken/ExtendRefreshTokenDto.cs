using System;

namespace BLL.Dtos.RefreshToken
{
    public class ExtendRefreshTokenDto : RefreshTokenDto
    {
        public DateTime AccessTokenExpiredDate { get; set; }
    }
}