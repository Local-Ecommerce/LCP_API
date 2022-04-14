namespace BLL.Dtos.News
{
    public class NewsRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool? Priority { get; set; }
        public string Type { get; set; }
        public string[] Image { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }
    }
}
