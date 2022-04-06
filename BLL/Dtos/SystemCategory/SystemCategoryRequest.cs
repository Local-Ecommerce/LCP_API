using System;

namespace BLL.Dtos.SystemCategory
{
    [Serializable]
    public class SystemCategoryRequest
    {
        public string SysCategoryName { get; set; }
        public string CategoryImage { get; set; }
        public string BelongTo { get; set; }
    }
}
