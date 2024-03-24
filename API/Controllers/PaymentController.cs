using BLL.Dtos;
using BLL.Dtos.Payment;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers {
	[EnableCors("MyPolicy")]
	[ApiController]
	[Route("api/payments")]
	public class PaymentController : ControllerBase {
		private readonly ILogger _logger;
		private readonly IPaymentService _paymentService;
		private readonly ITokenService _tokenService;

		public PaymentController(ILogger logger, IPaymentService paymentService, ITokenService tokenService) {
			_logger = logger;
			_paymentService = paymentService;
			_tokenService = tokenService;
		}


		/// <summary>
		/// Create payment (Customer)
		/// </summary>
		[Authorize(Roles = ResidentType.CUSTOMER)]
		[HttpPost]
		public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"POST api/payments START Request: " +
					$"{JsonSerializer.Serialize(paymentRequest)}");

			Stopwatch watch = new();
			watch.Start();

			//Create payment
			PaymentLinkResponse response = await _paymentService.CreatePayment(paymentRequest);

			string json = JsonSerializer.Serialize(ApiResponse<PaymentLinkResponse>.Success(response));

			watch.Stop();

			_logger.Information("POST api/payments END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}


		/// <summary>
		/// Get Payment (Authentication required)
		/// </summary>
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetPayment(
				[FromQuery] string id,
				[FromQuery] string orderid,
				[FromQuery] string paymentmethodid,
				[FromQuery] DateTime date,
				[FromQuery] int?[] status,
				[FromQuery] int? limit,
				[FromQuery] int? page,
				[FromQuery] string sort) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"GET api/payments?id={id}&orderid={orderid}" +
			$"&paymentmethodid={paymentmethodid}&date={date}&status=" + string.Join("status=", status) +
					$"&limit={limit}&page={page}&sort={sort} START");

			Stopwatch watch = new();
			watch.Start();

			//Get payment
			object response = await _paymentService.GetPayments(id, orderid, paymentmethodid, date, status, limit, page, sort);

			string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

			watch.Stop();

			_logger.Information($"GET api/payments?id={id}&orderid={orderid}" +
			$"&paymentmethodid={paymentmethodid}&date={date}&status=" + string.Join("status=", status) +
					$"&limit={limit}&page={page}&sort={sort} END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}
	}
}
