using BLL.Dtos;
using BLL.Dtos.Exception;
using DAL.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Filters
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var apiResponse = ApiResponse<string>.Fail(exception.Message);
            HttpResponseMessage response;

            switch (exception)
            {
                case BusinessException:
                    var businessException = (BusinessException)exception;
                    apiResponse.ResultCode = businessException.ErrorCode;
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    break;
                case EntityNotFoundException:
                    response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    break;
                case UnauthorizedAccessException:
                    apiResponse.ResultMessage = "Authentication failed";
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    break;
                case TimeoutException:
                    response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
                    break;
                case IllegalArgumentException:
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    break;
                default:
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    apiResponse.ResultMessage = CommonResponse.ERROR.ToString();
                    apiResponse.ResultCode = (int)CommonResponse.ERROR;
                    break;
            }

            response.Content = new StringContent(JsonSerializer.Serialize(apiResponse));
            context.Result = new HttpResponseMessageResult(response);
            base.OnException(context);
        }
    }

    public class HttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)_responseMessage.StatusCode;
            using var stream = await _responseMessage.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(context.HttpContext.Response.Body);
            await context.HttpContext.Response.Body.FlushAsync();
        }
    }
}
