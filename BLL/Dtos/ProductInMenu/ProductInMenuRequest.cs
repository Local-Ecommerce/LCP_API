using System;

namespace BLL.Dtos.ProductInMenu
{
    [Serializable]
    public class ProductInMenuRequest
    {
        public string ProductId { get; set; }
        public double Price { get; set; }
    }
}
