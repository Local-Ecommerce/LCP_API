namespace BLL.Dtos.LocalZone
{
    public class LocalZoneResponse
    {
        public string LocalZoneId { get; set; }
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public bool IsActive { get; set; }
    }
}
