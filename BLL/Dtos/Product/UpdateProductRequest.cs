using System;
using System.Collections.Generic;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class ExtendProductRequest : ProductRequest
    {
        public string ProductId { get; set; }
    }

    [Serializable]
    public class UpdateProductRequest
    {
        public List<ExtendProductRequest> Products { get; set; }
    }

}
