using System;

namespace BLL.Dtos.SystemCategory
{
    [Serializable]
    public class SystemCategoryRequest
    {
        public string SysCategoryName { get; set; }
        public string Type { get; set; }
        public string BelongTo { get; set; }
    }
}
