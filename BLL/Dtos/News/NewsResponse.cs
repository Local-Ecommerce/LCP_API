using System;

namespace BLL.Dtos.News
{
    public class NewsResponse
    {
        public string NewsId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool? Priority { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }
    }
}
