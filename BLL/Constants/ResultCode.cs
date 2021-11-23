namespace BLL.Constants
{
    public class ResultCode
    {
        /// <summary>
        /// ERROR CODE
        /// </summary>
        /// 

        public const int ERROR_CODE = -1;

        public const int SUCCESS_CODE = 0;

        public const int PRODUCT_NOT_FOUND_CODE = 1001;

        public const int DELETED_PRODUCT_CODE = 1002;

        /// <summary>
        /// ERROR MESSAGE
        /// </summary>

        public const string ERROR_MESSAGE = "Lỗi hệ thống";

        public const string SUCCESS_MESSAGE = "Thành công";

        public const string PRODUCT_NOT_FOUND_MESSAGE = "Không tìm thấy sản phẩm";

        public const string DELETED_PRODUCT_MESSAGE = "Sản phẩm đã bị xóa";


    }
}
