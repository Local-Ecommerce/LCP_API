namespace BLL.Constants
{
    public enum TimeUnit
    {
        THIRTY_DAYS = 30,
        ONE_HOUR = 1
    }

    public enum CategoryLevel
    {
        ONE = 1,
        TWO = 2,
        THREE = 3
    }

    public enum CommonResponse
    {
        ERROR = -1,
        SUCCESS = 0
    }

    public enum AccountStatus
    {
        ACTIVE_ACCOUNT = 1,
        INACTIVE_ACCOUNT = 2,
        ACCOUNT_NOT_FOUND = 3,
        DELETED_ACCOUNT = 4,
        INVALID_USERNAME_PASSWORD = 11,
        INVALID_PASSWORD = 12,
        INVALID_CONFIRM_PASSWORD = 13,
        UNAUTHORIZED_ACCOUNT = 14,
        ACCOUNT_ALREADY_EXISTS = 15,

    }

    public enum ProductStatus
    {
        PRODUCT_NOT_FOUND = 1003,
        DELETED_PRODUCT = 1004,
        VERIFIED_PRODUCT = 1005,
        UNVERIFIED_CREATE_PRODUCT = 1006,
        UNVERIFIED_UPDATE_PRODUCT = 1007
    }

    public enum MerchantStatus
    {
        MERCHANT_NOT_FOUND = 2003,
        DELETED_MERCHANT = 2004,
        VERIFIED_MERCHANT = 2005,
        UNVERIFIED_CREATE_MERCHANT = 2006,
        UNVERIFIED_UPDATE_MERCHANT = 2007,
        INVALID_NAME_MERCHANT = 2008,
        INVALID_PHONE_NUMBER_MERCHANT = 2009,
    }

    public enum SystemCategoryStatus
    {
        ACTIVE_SYSTEM_CATEGORY = 3001,
        INACTIVE_SYSTEM_CATEGORY = 3002,
        SYSTEM_CATEGORY_NOT_FOUND = 3003,
        DELETED_SYSTEM_CATEGORY = 3004,
        MAXED_OUT_LEVEL = 3016
    }

    public enum ApartmentStatus
    {
        ACTIVE_APARTMENT = 4001,
        INACTIVE_APARTMENT = 4002,
        APARTMENT_NOT_FOUND = 4003,
        DELETED_APARTMENT = 4004,
    }

    public enum ProductCategoryStatus
    {
        PRODUCT_CATEGORY_NOT_FOUND = 5003,
        DELETED_PRODUCT_CATEGORY = 5004,
        VERIFIED_PRODUCT_CATEGORY = 5005,
        UNVERIFIED_CREATE_PRODUCT_CATEGORY = 5006,
        UNVERIFIED_UPDATE_PRODUCT_CATEGORY = 5007,
    }

    public enum MerchantStoreStatus
    {
        MERCHANT_STORE_NOT_FOUND = 6003,
        DELETED_MERCHANT_STORE = 6004,
        VERIFIED_MERCHANT_STORE = 6005,
        UNVERIFIED_CREATE_MERCHANT_STORE = 6006,
        UNVERIFIED_UPDATE_MERCHANT_STORE = 6007,
    }

    public enum CustomerStatus
    {
        CUSTOMER_NOT_FOUND = 7003,
        DELETED_CUSTOMER = 7004,
        VERIFIED_CUSTOMER = 7005,
        UNVERIFIED_CREATE_CUSTOMER = 7006,
        UNVERIFIED_UPDATE_CUSTOMER = 7007,
        INVALID_NAME_CUSTOMER = 7008,
        INVALID_PHONE_NUMBER_CUSTOMER = 7009,
        INVALID_DATE_OF_BIRTH = 7010
    }

    public enum CollectionStatus
    {
        ACTIVE_COLLECTION = 8001,
        INACTIVE_COLLECTION = 8002,
        COLLECTION_NOT_FOUND = 8003,
        DELETED_COLLECTION = 8004,
    }

    public enum CollectionMappingStatus
    {
        ACTIVE_PRODUCT = 9001,
        INACTIVE_PRODUCT = 9002,
        PRODUCT_NOT_FOUND = 9003,
    }

    public enum DeliveryAddressStatus
    {
        ACTIVE_DELIVERYADDRESS = 10001,
        INACTIVE_DELIVERYADDRESS = 10002,
        DELIVERYADDRESS_NOT_FOUND = 10003,
        DELETED_DELIVERYADDRESS = 10004
    }
    public enum MarketManagerStatus
    {
        ACTIVE_MARKETMANAGER = 11001,
        INACTIVE_MARKETMANAGER = 11002,
        MARKETMANAGER_NOT_FOUND = 11003,
        DELETED_MARKETMANAGER = 11004,
        INVALID_NAME_MARKETMANAGER = 11008,
        INVALID_PHONE_NUMBER_MARKETMANAGER = 11009,
    }

    public enum NewsStatus
    {
        ACTIVE_NEWS = 12001,
        INACTIVE_NEWS = 12002,
        NEWS_NOT_FOUND = 12003
    }

    public enum PoiStatus
    {
        ACTIVE_POI = 13001,
        INACTIVE_POI = 13002,
        POI_NOT_FOUND = 13003
    }

    public enum MenuStatus
    {
        ACTIVE_MENU = 14001,
        INACTIVE_MENU = 14002,
        MENU_NOT_FOUND = 14003,
        DELETED_MENU = 14004
    }

    public enum ProductInMenuStatus
    {
        ACTIVE_PRODUCT_IN_MENU = 15001,
        INACTIVE_PRODUCT_IN_MENU = 15002,
        PRODUCT_IN_MENU_NOT_FOUND = 15003
    }

    public enum StoreMenuDetailStatus
    {
        ACTIVE_STORE_MENU_DETAIL = 16001,
        INACTIVE_STORE_MENU_DETAIL = 16002,
        STORE_MENU_DETAIL_NOT_FOUND = 16003,
        DELETED_STORE_MENU_DETAIL = 16004
    }

    public enum PaymentStatus
    {
        ACTIVE_PAYMENT = 17001,
        INACTIVE_PAYMENT = 17002,
        PAYMENT_NOT_FOUND = 17003,
        DELETED_PAYMENT = 17004
    }

    public enum PaymentMethodStatus
    {
        ACTIVE_PAYMENT_METHOD = 18001,
        INACTIVE_PAYMENT_METHOD = 18002,
        PAYMENT_METHOD_NOT_FOUND = 18003,
        DELETED_PAYMENT_METHOD = 18004
    }

    public enum ResidentStatus
    {
        ACTIVE_RESIDENT = 19001,
        INACTIVE_RESIDENT = 19002,
        RESIDENT_NOT_FOUND = 19003,
        DELETED_RESIDENT = 19004,
        INVALID_NAME_RESIDENT = 19008,
        INVALID_PHONE_NUMBER_RESIDENT = 19009,
        INVALID_DATE_OF_BIRTH_RESIDENT = 19010,
        INVALID_TYPE_RESIDENT = 19011,
    }

    public enum OrderStatus
    {
        CART = 20001,
        ORDER_NOT_FOUND = 20003,
        DELETED_ORDER = 20004,
        PAID = 20005,
        IN_PAYMENT = 20006
    }
}
