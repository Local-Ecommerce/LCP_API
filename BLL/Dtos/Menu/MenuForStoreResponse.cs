using System;

namespace BLL.Dtos.Menu
{
    public class MenuForStoreResponse
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
    }
}
