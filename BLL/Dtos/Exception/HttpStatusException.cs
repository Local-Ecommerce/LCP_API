using System.Net;

namespace BLL.Dtos.Exception
{
    public class HttpStatusException : System.Exception
    {
        public HttpStatusCode Status { get; private set; }

        public object Response { get; set; }

        public HttpStatusException(HttpStatusCode status, object response)
        {
            Status = status;
            Response = response;
        }
    }
}
