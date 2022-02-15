using BLL.Dtos;
using BLL.Dtos.PaymentMethod;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/paymentMethod")]
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
            _logger.Information($"POST api/paymentMethod START Request: " +
                $"{JsonSerializer.Serialize(paymentMethodRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create PaymentMethod
            BaseResponse<PaymentMethodResponse> response = await _paymentMethodService.CreatePaymentMethod(paymentMethodRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/paymentMethod END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Payment Method By Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethodById(string id)
        {
            _logger.Information($"GET api/paymentMethod/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get PaymentMethod
            BaseResponse<PaymentMethodResponse> response = await _paymentMethodService.GetPaymentMethodById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/paymentMethod/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Payment Method By Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentMethodById(string id, [FromBody] PaymentMethodRequest paymentMethodRequest)
        {
            _logger.Information($"PUT api/paymentMethod/{id} START Request: " +
                $"{JsonSerializer.Serialize(paymentMethodRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update PaymentMethod
            BaseResponse<PaymentMethodResponse> response = await _paymentMethodService.UpdatePaymentMethodById(id, paymentMethodRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/paymentMethod/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Payment Method By Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethodById(string id)
        {
            _logger.Information($"DELETE api/paymentMethod/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete PaymentMethod
            BaseResponse<PaymentMethodResponse> response = await _paymentMethodService.DeletePaymentMethod(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"DELETE api/paymentMethod/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get All Payment Method
        /// </summary>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPaymentMethod()
        {
            _logger.Information($"GET api/paymentMethod/all START");

            Stopwatch watch = new();
            watch.Start();

            //Get PaymentMethod
            BaseResponse<List<PaymentMethodResponse>> responses = await _paymentMethodService.GetAllPaymentMethod();

            string json = JsonSerializer.Serialize(responses);

            watch.Stop();

            _logger.Information($"GET api/paymentMethod/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
