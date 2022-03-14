using BLL.Dtos;
using BLL.Dtos.PaymentMethod;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

        public PaymentMethodController(ILogger logger, IPaymentMethodService paymentMethodService)
        {
            _logger = logger;
            _paymentMethodService = paymentMethodService;
        }


        /// <summary>
        /// Create Payment Method (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethodRequest paymentMethodRequest)
        {
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
        /// Get Payment Method
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethod(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort)
        {
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
