using System;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class UpdateProductRequest : ProductRequest
    {
        public string ProductId { get; set; }
    }
}
