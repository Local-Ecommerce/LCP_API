using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class BaseProductRequest : ProductRequest
    {
        public Collection<ProductRequest> RelatedProducts { get; set; }
        public List<string> SystemCategoryIds { get; set; }
    }
}