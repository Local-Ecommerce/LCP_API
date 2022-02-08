using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.SystemCategory
{
    [Serializable]
    public class SystemCategoryResponse
    {
        public string SystemCategoryId { get; set; }
        public string SysCategoryName { get; set; }
        public string ApproveBy { get; set; }
        public int? Status { get; set; }
        public int CategoryLevel { get; set; }
        public string BelongTo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<SystemCategoryResponse> InverseBelongToNavigation { get; set; }
    }
}
