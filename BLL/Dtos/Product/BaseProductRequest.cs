using System;
using System.Collections.ObjectModel;
using BLL.Dtos.ProductCategory;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class BaseProductRequest : ProductRequest
    {
        public Collection<ProductRequest> InverseBelongToNavigation { get; set; }
        public Collection<ProductCategoryRequest> ProductCategories { get; set; }
    }
}