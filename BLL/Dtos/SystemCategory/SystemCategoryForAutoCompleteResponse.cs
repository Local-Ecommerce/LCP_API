using System;

namespace BLL.Dtos.SystemCategory
{
    [Serializable]
    public class SystemCategoryForAutoCompleteResponse
    {
        public string SystemCategoryId { get; set; }
        public string SysCategoryName { get; set; }
        public int CategoryLevel { get; set; }
    }
}
