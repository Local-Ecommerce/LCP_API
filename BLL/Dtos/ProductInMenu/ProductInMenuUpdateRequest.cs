using System;
using System.Collections.Generic;

namespace BLL.Dtos.ProductInMenu
{
    [Serializable]
    public class ProductInMenuUpdateRequest
    {
        public string ProductInMenuId { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public int? Quantity { get; set; }
        public int? MaxBuy { get; set; }
    }


    [Serializable]
    public class ListProductInMenuUpdateRequest
    {
        public List<ProductInMenuUpdateRequest> ProductInMenus { get; set; }
    }
}
