using System;

namespace BLL.Dtos.ProductCombination
{
    public class ProductCombinationRequest
    {
        public string BaseProductId { get; set; }
        public string ProductId { get; set; }
        public int? DefaultMin { get; set; }
        public string DefaultMax { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? Status { get; set; }
    }
}
