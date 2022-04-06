using System;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class MenuUpdateRequest
    {
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int? Status { get; set; }
        public string RepeatDate { get; set; }
        public bool? IncludeBaseMenu { get; set; }
    }
}