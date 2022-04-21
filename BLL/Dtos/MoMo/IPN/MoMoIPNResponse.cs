namespace BLL.Dtos.MoMo.IPN
{
    public class MoMoIPNResponse
    {
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public long ResponseTime { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
    }
}
