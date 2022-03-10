using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.SystemCategory
{
    [Serializable]
    public class ParentSystemCategoryResponse : SystemCategoryResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Collection<ParentSystemCategoryResponse> Children { get; set; }
    }

    [Serializable]
    public class ChildSystemCategoryResponse : SystemCategoryResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SystemCategoryResponse Parent { get; set; }
    }
}