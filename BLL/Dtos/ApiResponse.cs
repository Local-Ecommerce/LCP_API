using DAL.Constants;
using System.Text.Json.Serialization;

namespace BLL.Dtos {
	public class ApiResponse<T> {
		public static ApiResponseFailed<T> Fail(int code, string message) {
			return new ApiResponseFailed<T> { ResultCode = code, ResultMessage = message };
		}

		public static ApiResponseFailed<T> Fail(string message) {
			return new ApiResponseFailed<T> { ResultCode = (int)CommonResponse.ERROR, ResultMessage = message };
		}

		public static ApiResponseSucess<T> Success(T data) {
			return new ApiResponseSucess<T> {
				ResultCode = (int)CommonResponse.SUCCESS,
				ResultMessage = CommonResponse.SUCCESS.ToString(),
				Data = data
			};
		}

		public static ApiResponseSucess<T> Success() {
			return new ApiResponseSucess<T> { ResultCode = (int)CommonResponse.SUCCESS, ResultMessage = CommonResponse.SUCCESS.ToString() };
		}
	}

	public class ApiResponseSucess<T> {
		public int ResultCode { get; set; }
		public string ResultMessage { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public T Data { get; set; }
	}

	public class ApiResponseFailed<T> {
		public int ResultCode { get; set; }
		public string ResultMessage { get; set; }
	}
}
