using System;

namespace BLL.Dtos.Collection
{
    [Serializable]
    public class CollectionRequest
    {
        public string CollectionName { get; set; }
        public string ResidentId { get; set; }

    }
}
