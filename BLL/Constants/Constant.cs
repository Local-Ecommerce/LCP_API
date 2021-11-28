namespace BLL.Constants
{
    public enum TimeUnit
    {
        THIRTY_DAYS = 30
    }

    public enum CommonResponse
    {
        ERROR = -1,
        SUCCESS = 0
    }

    public enum AccountStatus
    {
        ACCOUNT_NOT_FOUND = 1,
        UNAUTHORIZED_ACCOUNT = 2,
        ACCOUNT_ALREADY_EXISTS = 3,
        INVALID_USERNAME_PASSWORD = 4,
        INVALID_CONFIRM_PASSWORD = 5,
        DELETED_ACCOUNT = 6,
    }

    public enum ProductStatus
    {
        PRODUCT_NOT_FOUND = 1001,
        DELETED_PRODUCT = 1002,
        UNVERIFIED_CREATE_PRODUCT = 1003,
        UNVERIFIED_UPDATE_PRODUCT = 1004
    }

    public enum MerchantStatus
    {
        MERCHANT_NOT_FOUND = 2001,
        DELETED_MERCHANT = 2002,
        UNVERIFIED_CREATE_MERCHANT = 2003,
        UNVERIFIED_UPDATE_MERCHANT = 2004
    }

    public enum SystemCategoryStatus
    {
        SYSTEM_CATEGORY_NOT_FOUND = 3001
    }

    public enum LocalZoneStatus
    {
        LOCALZONE_NOT_FOUND = 4001,
        DELETED_LOCALZONE = 4002,
        UNVERIFIED_CREATE_LOCALZONE = 4003,
        UNVERIFIED_UPDATE_LOCALZONE = 4004
    }

    public enum ProductCategoryStatus
    {
        PRODUCT_CATEGORY_NOT_FOUND = 5001,
        UNVERIFIED_CREATE_PRODUCT_CATEGORY = 5002,
        UNVERIFIED_UPDATE_PRODUCT_CATEGORY = 5003,
        DELETED_PRODUCT_CATEGORY = 5004
    }

    public enum MerchantStoreStatus
    {
        MERCHANTSTORE_NOT_FOUND = 6001,
        DELETED_MERCHANTSTORE = 6006,
        UNVERIFIED_CREATE_MERCHANTSTORE = 6003,
        UNVERIFIED_UPDATE_MERCHANTSTORE = 6004
    }

    public enum CustomerStatus
    {
        CUSTOMER_NOT_FOUND = 7001,
        DELETED_CUSTOMER = 7002,
        UNVERIFIED_CREATE_CUSTOMER = 7003,
        UNVERIFIED_UPDATE_CUSTOMER = 7007
    }
}
