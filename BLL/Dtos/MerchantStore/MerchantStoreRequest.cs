namespace BLL.Dtos.MerchantStore
{
    public class MerchantStoreRequest
    {
        public string StoreName { get; set; }
        public string MerchantId { get; set; }
        public string LocalZoneId { get; set; }
    }

    public enum MerchantStoreStatus
    {
        ERROR = -1,
        SUCCESS = 0,
        MERCHANTSTORE_NOT_FOUND = 6001,
        DELETED_MERCHANTSTORE = 6006,
        UNVERIFIED_CREATE_MERCHANTSTORE = 6003,
        UNVERIFIED_UPDATE_MERCHANTSTORE = 6004
    }
}
