using System;

namespace BLL.Dtos.Product
{
    [Serializable]
    public class UpdateProductRequest
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public double DefaultPrice { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public string Image { get; set; }
    }
}
