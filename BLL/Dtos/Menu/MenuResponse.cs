using System;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class MenuResponse
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
    }
}
