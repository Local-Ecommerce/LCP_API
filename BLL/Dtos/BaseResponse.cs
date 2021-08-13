using System;
using System.Text.Json.Serialization;

namespace BLL.Dtos
{
    [Serializable]
    public class BaseResponse<T> where T : class
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Data { get; set; }

    }
}
