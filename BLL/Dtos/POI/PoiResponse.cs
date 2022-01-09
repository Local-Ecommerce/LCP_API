using System;

namespace BLL.Dtos.POI
{
    public class PoiResponse
    {
        public string PoiId { get; set; }
        public DateTime RealeaseDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }
        public string MarketManagerId { get; set; }
        public string AparmentId { get; set; }
    }
}
