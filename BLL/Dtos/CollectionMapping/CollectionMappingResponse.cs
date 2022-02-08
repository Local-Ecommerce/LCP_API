using System;

namespace BLL.Dtos.CollectionMapping
{
    [Serializable]
    public class CollectionMappingResponse
    {
        public string CollectionId { get; set; }
        public string ProductId { get; set; }
        public int Status { get; set; }
    }
}
