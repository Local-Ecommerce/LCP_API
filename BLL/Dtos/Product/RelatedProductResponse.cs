namespace BLL.Dtos.Product
{
    public class RelatedProductResponse : ProductResponse
    {
        public ProductResponse BaseProduct { get; set; }
    }
}