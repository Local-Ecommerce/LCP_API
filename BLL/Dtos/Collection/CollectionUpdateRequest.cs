using System;

namespace BLL.Dtos.Collection
{
    [Serializable]
    public class CollectionUpdateRequest
    {
        public string CollectionName { get; set; }
        public int? Status { get; set; }
    }
}