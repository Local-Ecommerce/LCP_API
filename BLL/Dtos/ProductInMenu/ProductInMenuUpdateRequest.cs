using System;

namespace BLL.Dtos.ProductInMenu
{
    [Serializable]
    public class ProductInMenuUpdateRequest
    {
        public double Price { get; set; }
        public int Status { get; set; }

    }
}
