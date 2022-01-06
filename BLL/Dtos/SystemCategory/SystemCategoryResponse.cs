using System.Collections.Generic;

namespace BLL.Dtos.SystemCategory
{
    public class SystemCategoryResponse
    {
        public string SystemCategoryId { get; set; }
        public string SysCategoryName { get; set; }
        public string ApproveBy { get; set; }
        public int? Status { get; set; }
        public int CategoryLevel { get; set; }
        public string BelongTo { get; set; }
        public List<SystemCategoryResponse> Child { get; set; }
    }
}
