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

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/feedbacks")]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IFeedbackService _feedbackService;
        private readonly ITokenService _tokenService;

        public FeedbackController(ILogger logger, IFeedbackService feedbackService, ITokenService tokenService)
        {
            _logger = logger;
            _feedbackService = feedbackService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Send feedback (Customer)
        /// </summary>
        [Authorize(Roles = ResidentType.CUSTOMER)]
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] FeedbackRequest feedbackRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"POST api/feedbacks START Request: " +
                $"{JsonSerializer.Serialize(feedbackRequest)}");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName[(claimName.LastIndexOf(':') + 2)..];

            //Create Feedback
            FeedbackResponse response = await _feedbackService.CreateFeedback(feedbackRequest, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<FeedbackResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/feedback END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Feedback (Market Manager, Customer, Merchant)
        /// </summary>
        [AuthorizeRoles(ResidentType.CUSTOMER, ResidentType.MARKET_MANAGER, ResidentType.MERCHANT)]
        [HttpGet]
        public async Task<IActionResult> GetMenu(
            [FromQuery] string id,
            [FromQuery] string residentid,
            [FromQuery] string productid,
            [FromQuery] double? rating,
            [FromQuery] DateTime? date,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string[] include)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/feedbacks?id={id}&productid={productid}" +
                $"&residentid={residentid}&rating={rating}&date={date}" +
                $"&limit={limit}&page={page}&sort={sort}&include="
                + string.Join("include=", include) + " START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentSendRequest = claimName[(claimName.LastIndexOf(':') + 2)..];

            //get role from token
            string claimRole = claim.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().ToString();
            string role = claimRole.Substring(claimRole.LastIndexOf(':') + 2);

            //get Feedback
            object responses = await _feedbackService
                .GetFeedback(id, productid, residentid, residentSendRequest, role, rating, date, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(responses));

            watch.Stop();

            _logger.Information($"GET api/feedbacks?id={id}&productid={productid}" +
                $"&residentid={residentid}&rating={rating}&date={date}" +
                $"&limit={limit}&page={page}&sort={sort}&include="
                + string.Join("include=", include) + " END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}