using System;

namespace BLL.Dtos.ProductInMenu
{
    public class ProductQuantityDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime UpdatedDate { get; set; }


        public ProductQuantityDto(string productId, int quantity, DateTime updatedDate)
        {
            this.ProductId = productId;
            this.Quantity = quantity;
            this.UpdatedDate = updatedDate;
        }
    }
}