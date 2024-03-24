using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.Feedback;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace API.Controllers {
	[EnableCors("MyPolicy")]
	[ApiController]
	[Route("api/feedbacks")]
	public class FeedbackController : ControllerBase {
		private readonly ILogger _logger;
		private readonly IFeedbackService _feedbackService;
		private readonly ITokenService _tokenService;

		public FeedbackController(ILogger logger, IFeedbackService feedbackService, ITokenService tokenService) {
			_logger = logger;
			_feedbackService = feedbackService;
			_tokenService = tokenService;
		}

		/// <summary>
		/// Send feedback (Customer)
		/// </summary>
		[Authorize(Roles = ResidentType.CUSTOMER)]
		[HttpPost]
		public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequest feedbackRequest) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"POST api/feedbacks START Request: " +
					$"{JsonSerializer.Serialize(feedbackRequest)}");

			Stopwatch watch = new();
			watch.Start();

			var identity = HttpContext.User.Identity as ClaimsIdentity;
			(string residentId, _) = _tokenService.GetResidentIdAndRole(identity);

			//Create Feedback
			FeedbackResponse response = await _feedbackService.CreateFeedback(feedbackRequest, residentId);

			string json = JsonSerializer.Serialize(ApiResponse<FeedbackResponse>.Success(response));

			watch.Stop();

			_logger.Information("POST api/feedbacks END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}

		/// <summary>
		/// Get Feedback (Market Manager, Customer, Merchant)
		/// </summary>
		[AuthorizeRoles(ResidentType.CUSTOMER, ResidentType.MARKET_MANAGER, ResidentType.MERCHANT)]
		[HttpGet]
		public async Task<IActionResult> GetFeedback(GetFeedbackRequest request) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"GET api/feedbacks START. Params: {request}");

			Stopwatch watch = new();
			watch.Start();

			var identity = HttpContext.User.Identity as ClaimsIdentity;
			(string residentSendRequest, string role) = _tokenService.GetResidentIdAndRole(identity);

			//get Feedback
			object responses = await _feedbackService.GetFeedback(request, role, residentSendRequest);

			string json = JsonSerializer.Serialize(ApiResponse<object>.Success(responses));

			watch.Stop();

			_logger.Information($"GET api/feedbacks END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}


		/// <summary>
		/// Read Feedback (Market Manager)
		/// </summary>
		[Authorize(Roles = ResidentType.MARKET_MANAGER)]
		[HttpPut]
		public async Task<IActionResult> ReadFeedback([FromQuery] string id) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"PUT api/feedbacks?id={id} START Request: ");

			Stopwatch watch = new();
			watch.Start();

			//Read Feedback
			FeedbackResponse response = await _feedbackService.ReadFeedback(id);

			string json = JsonSerializer.Serialize(ApiResponse<FeedbackResponse>.Success(response));

			watch.Stop();

			_logger.Information($"PUT api/feedbacks?id={id} END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}
	}
}