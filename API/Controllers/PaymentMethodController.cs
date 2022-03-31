using BLL.Dtos;
using BLL.Dtos.PaymentMethod;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/payment-methods")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly ITokenService _tokenService;

        public PaymentMethodController(ILogger logger, IPaymentMethodService paymentMethodService,
        ITokenService tokenService)
        {
            _logger = logger;
            _paymentMethodService = paymentMethodService;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Create Payment Method (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethodRequest paymentMethodRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"POST api/payment-methods START Request: " +
                $"{JsonSerializer.Serialize(paymentMethodRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create PaymentMethod
            PaymentMethodResponse response = await _paymentMethodService.CreatePaymentMethod(paymentMethodRequest);

            string json = JsonSerializer.Serialize(ApiResponse<PaymentMethodResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/payment-methods END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Payment Method (Authorization required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethod(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/payment-methods?id={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort} START");

            Stopwatch watch = new();
            watch.Start();

            //Get PaymentMethod
            object response = await _paymentMethodService.GetPaymentMethods(id, status, limit, page, sort);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/payment-methods?id={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Payment Method By Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut]
        public async Task<IActionResult> UpdatePaymentMethodById([FromQuery] string id,
        [FromBody] PaymentMethodRequest paymentMethodRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/payment-methods?id={id} START Request: " +
                $"{JsonSerializer.Serialize(paymentMethodRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update PaymentMethod
            PaymentMethodResponse response = await _paymentMethodService.UpdatePaymentMethodById(id, paymentMethodRequest);

            string json = JsonSerializer.Serialize(ApiResponse<PaymentMethodResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/payment-methods?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Payment Method By Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeletePaymentMethodById([FromQuery] string id)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"DELETE api/payment-methods?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete PaymentMethod
            await _paymentMethodService.DeletePaymentMethod(id);

            string json = JsonSerializer.Serialize(ApiResponse<PaymentMethodResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/payment-methods?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
