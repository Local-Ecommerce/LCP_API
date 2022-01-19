namespace BLL.Dtos.POI
{
    public class PoiUpdateRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }
    }
}