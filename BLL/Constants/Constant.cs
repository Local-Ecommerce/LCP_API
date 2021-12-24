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
        ACTIVE_ACCOUNT = 7,
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
        SYSTEM_CATEGORY_NOT_FOUND = 3001,
        ACTIVE_SYSTEM_CATEGORY = 3002,
        DELETED_SYSTEM_CATEGORY = 3003
    }

    public enum ApartmentStatus
    {
        APARTMENT_NOT_FOUND = 4001,
        DELETED_APARTMENT = 4002,
        ACTIVE_APARTMENT = 4003
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
        MERCHANT_STORE_NOT_FOUND = 6001,
        DELETED_MERCHANT_STORE = 6006,
        UNVERIFIED_CREATE_MERCHANT_STORE = 6003,
        UNVERIFIED_UPDATE_MERCHANT_STORE = 6004
    }

    public enum CustomerStatus
    {
        CUSTOMER_NOT_FOUND = 7001,
        DELETED_CUSTOMER = 7002,
        UNVERIFIED_CREATE_CUSTOMER = 7003,
        UNVERIFIED_UPDATE_CUSTOMER = 7004
    }

    public enum CollectionStatus
    {
        COLLECTION_NOT_FOUND = 8001,
        DELETED_COLLECTION = 8002,
        ACTIVE_COLLECTION = 8003
    }

    public enum CollectionMappingStatus
    {
        PRODUCT_NOT_FOUND = 9001,
        ACTIVE_PRODUCT = 9002,
        INACTIVE_PRODUCT = 9003
    }

    public enum DeliveryAddressStatus
    {
        DELIVERYADDRESS_NOT_FOUND = 10001,
        DELETED_DELIVERYADDRESS = 10002,
        ACTIVE_DELIVERYADDRESS = 10003
    }
    public enum MarketManagerStatus
    {
        MARKETMANAGER_NOT_FOUND = 11001,
        DELETED_MARKETMANAGER = 11002,
        ACTIVE_MARKETMANAGER = 11003
    }

    public enum NewsStatus
    {
        ACTIVE_NEWS = 12001,
        DEACTIVE_NEWS = 12002,
        NEWS_NOT_FOUND = 12003
    }
}
