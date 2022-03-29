using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class BaseProductRequest : ProductRequest
    {
        public bool ToBaseMenu { get; set; }
        public Collection<ProductRequest> RelatedProducts { get; set; }
    }
}