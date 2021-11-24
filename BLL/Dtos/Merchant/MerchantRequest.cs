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
        PRODUCT_NOT_FOUND = 1001,
        DELETED_PRODUCT = 1002,
        UNVERIFIED_CREATE_PRODUCT = 1003,
        UNVERIFIED_UPDATE_PRODUCT = 1004
    }
}
