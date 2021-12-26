namespace BLL.Dtos.New
{
    public class NewsRequest
    {
        public string Text { get; set; }
        public int? Status { get; set; }
        public string MarketManagerId { get; set; }
        public string ApartmentId { get; set; }
    }
}
