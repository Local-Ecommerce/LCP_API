using DAL.Constants;

namespace BLL.Dtos.Exception
{
    public class BusinessException : System.Exception
    {
        public int ErrorCode { get; set; }

        public BusinessException() : base("Lỗi nhập liệu")
        {
        }

        public BusinessException(string errorMessage, int errorCode = (int)CommonResponse.ERROR) : base(errorMessage)
        {
            ErrorCode = errorCode;
        }
    }
}
