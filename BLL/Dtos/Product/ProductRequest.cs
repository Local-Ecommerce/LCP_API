using System;
using System.Collections.Generic;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class ProductRequest
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public double DefaultPrice { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public string SystemCategoryId { get; set; }
        public string[] Image { get; set; }
    }

    [Serializable]
    public class ProductIdsRequest
    {
        public List<string> ProductIds { get; set; }
    }


    [Serializable]
    public class ListProductRequest
    {
        public List<ProductRequest> Products { get; set; }
    }
}
