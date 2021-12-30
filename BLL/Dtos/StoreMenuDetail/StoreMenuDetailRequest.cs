using System;

namespace BLL.Dtos.StoreMenuDetail
{
    public class StoreMenuDetailRequest
    {
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string MenuId { get; set; }
    }
}
