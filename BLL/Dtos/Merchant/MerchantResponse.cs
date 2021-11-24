namespace BLL.Dtos.Merchant
{
    public class MerchantResponse
    {
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlock { get; set; }
        public string AccountId { get; set; }
        public string LevelId { get; set; }
    }
}
