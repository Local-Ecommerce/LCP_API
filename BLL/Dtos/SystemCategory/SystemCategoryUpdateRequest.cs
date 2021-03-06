using System;

namespace BLL.Dtos.SystemCategory
{
    [Serializable]
    public class SystemCategoryUpdateRequest
    {
        public string SysCategoryName { get; set; }
        public string CategoryImage { get; set; }
        public int? Status { get; set; }
        public string BelongTo { get; set; }
    }
}