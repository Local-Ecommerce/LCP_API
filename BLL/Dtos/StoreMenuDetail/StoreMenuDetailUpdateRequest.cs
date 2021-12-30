using System;

namespace BLL.Dtos.StoreMenuDetail
{
    public class StoreMenuDetailUpdateRequest
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int Status { get; set; }
    }
}
