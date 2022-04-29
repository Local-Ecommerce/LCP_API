namespace DAL.Constants
{
    public enum TimeUnit
    {
        THIRTY_DAYS = 30,
        ONE_HOUR = 1,
        TWENTY_FOUR_HOUR = 24,
        TIMEOUT_20_SEC = 20000,
        READ_WRITE_TIMEOUT = 30000
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
        DELETED_ACCOUNT = 2,
        INVALID_FIREBASE_TOKEN = 11,
        ACCOUNT_ALREADY_EXISTS = 12,
        UNAUTHORIZED_ACCOUNT = 13
    }

    public enum ProductStatus
    {
        VERIFIED_PRODUCT = 1001,
        DELETED_PRODUCT = 1002,
        REJECTED_PRODUCT = 1003,
        UNVERIFIED_PRODUCT = 1004
    }

    public enum MerchantStatus
    {
        VERIFIED_MERCHANT = 2001,
        DELETED_MERCHANT = 2002,
        REJECTED_MERCHANT = 2003,
        UNVERIFIED_MERCHANT = 2004
    }

    public enum SystemCategoryStatus
    {
        ACTIVE_SYSTEM_CATEGORY = 3001,
        DELETED_SYSTEM_CATEGORY = 3002,
        MAXED_OUT_LEVEL = 3010,
        INACTIVE_SYSTEM_CATEGORY = 3005
    }

    public enum ApartmentStatus
    {
        ACTIVE_APARTMENT = 4001,
        INACTIVE_APARTMENT = 4005
    }

    public enum OrderStatus
    {
        OPEN = 5001,
        CANCELED_ORDER = 5002,
        CONFIRMED = 5006,
        COMPLETED = 5007
    }

    public enum MerchantStoreStatus
    {
        VERIFIED_MERCHANT_STORE = 6001,
        DELETED_MERCHANT_STORE = 6002,
        REJECTED_MERCHANT_STORE = 6003,
        UNVERIFIED_MERCHANT_STORE = 6004
    }

    public enum NewsStatus
    {
        ACTIVE_NEWS = 7001,
        INACTIVE_NEWS = 7005,
    }

    public enum PoiStatus
    {
        ACTIVE_POI = 8001,
        INACTIVE_POI = 8005
    }

    public enum MenuStatus
    {
        ACTIVE_MENU = 9001,
        DELETED_MENU = 9002,
        INACTIVE_MENU = 9005
    }

    public enum ProductInMenuStatus
    {
        ACTIVE_PRODUCT_IN_MENU = 10001,
        DELETED_PRODUCT_IN_MENU = 10002,
        INACTIVE_PRODUCT_IN_MENU = 10005
    }

    public enum MoMoStatus
    {
        MOMO_IPN_SIGNATURE_NOT_MATCH = 11008
    }

    public enum PaymentStatus
    {
        PAID = 12001,
        FAILED = 12002,
        UNPAID = 12005
    }

    public enum PaymentMethodStatus
    {
        ACTIVE_PAYMENT_METHOD = 13001,
        DELETED_PAYMENT_METHOD = 13002,
        INACTIVE_PAYMENT_METHOD = 13005
    }

    public enum ResidentStatus
    {
        VERIFIED_RESIDENT = 14001,
        DELETED_RESIDENT = 14002,
        REJECTED_RESIDENT = 14003,
        UNVERIFIED_RESIDENT = 14004,
        INACTIVE_RESIDENT = 14005,
        INVALID_DATE_OF_BIRTH_RESIDENT = 14009
    }

    public enum NotificationCode
    {
        PAYMENT = 301,
        WARNING = 103
    }
}
