namespace BLL.Dtos.Merchant
{
    public class MerchantRequest
    {
        public string MerchantName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountId { get; set; }
    }

    public enum MerchantStatus
    {
        ERROR = -1,
        SUCCESS = 0,
        MERCHANT_NOT_FOUND = 2001,
        DELETED_MERCHANT = 2002,
        UNVERIFIED_CREATE_MERCHANT = 2003,
        UNVERIFIED_UPDATE_MERCHANT = 2004
    }
}
