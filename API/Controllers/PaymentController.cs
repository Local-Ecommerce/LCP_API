using BLL.Dtos;
using BLL.Dtos.Payment;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }


        /// <summary>
        /// Create payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest)
        {
            _logger.Information($"POST api/payment/create START Request: " +
                $"{JsonSerializer.Serialize(paymentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create payment
            BaseResponse<PaymentResponse> response = await _paymentService.CreatePayment(paymentRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/payment/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(string id)
        {
            _logger.Information($"GET api/payment/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get payment
            BaseResponse<PaymentResponse> response = await _paymentService.GetPaymentById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/payment/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePaymentById(string id, [FromBody] PaymentRequest paymentRequest)
        {
            _logger.Information($"PUT api/payment/update/{id} START Request: " +
                $"{JsonSerializer.Serialize(paymentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update payment
            BaseResponse<PaymentResponse> response = await _paymentService.UpdatePaymentById(id, paymentRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/payment/update/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeletePaymentById(string id)
        {
            _logger.Information($"PUT api/payment/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete payment
            BaseResponse<PaymentResponse> response = await _paymentService.DeletePaymentById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/payment/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Payment By Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(string orderId)
        {
            _logger.Information($"GET api/payment/apartment/{orderId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get payment by OrderId
            BaseResponse<List<PaymentResponse>> response = await _paymentService.GetPaymentByOrderId(orderId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/payment/apartment/{orderId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get Payment By Payment Method Id
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("paymentMethod/{paymentMethodId}")]
        public async Task<IActionResult> GetPaymentByPaymentMethodId(string paymentMethodId)
        {
            _logger.Information($"GET api/payment/paymentMethod/{paymentMethodId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get payment by paymentMethod
            BaseResponse<List<PaymentResponse>> response = await _paymentService.GetPaymentByPaymentMethodId(paymentMethodId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/payment/paymentMethod/{paymentMethodId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get Payment By Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("bydate/{date}")]
        public async Task<IActionResult> GetPaymentByDate(DateTime date)
        {
            _logger.Information($"GET api/payment/{date} START");

            Stopwatch watch = new();
            watch.Start();

            //Get payment by RealeaseDate
            BaseResponse<List<PaymentResponse>> response = await _paymentService.GetPaymentByDate(date);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/payment/{date} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
