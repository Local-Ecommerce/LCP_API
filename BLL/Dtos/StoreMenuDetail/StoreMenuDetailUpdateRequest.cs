using System;

namespace BLL.Dtos.StoreMenuDetail
{
    public class StoreMenuDetailUpdateRequest
    {
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public int? Status { get; set; }
        public string RepeatDate { get; set; }
    }
}
