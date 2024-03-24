using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using API.Extensions;
using BLL.Dtos;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace API.Controllers {
	[EnableCors("MyPolicy")]
	[ApiController]
	[Route("api/dashboard")]
	public class DashboardController : ControllerBase {
		private readonly ILogger _logger;
		private readonly IDashboardService _dashboardService;
		private readonly ITokenService _tokenService;

		public DashboardController(
				ILogger logger,
				IDashboardService dashboardService,
				ITokenService tokenService) {
			_logger = logger;
			_dashboardService = dashboardService;
			_tokenService = tokenService;
		}


		/// <summary>
		/// Get Dashboard (Admin, Market Manager, Merchant)
		/// </summary>
		[AuthorizeRoles(ResidentType.MARKET_MANAGER, ResidentType.MERCHANT, RoleId.ADMIN)]
		[HttpGet]
		public async Task<IActionResult> GetApartment(
				[FromQuery] int days) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"GET api/dashboard?days={days} START");

			Stopwatch watch = new();
			watch.Start();

			var identity = HttpContext.User.Identity as ClaimsIdentity;
			IEnumerable<Claim> claim = identity.Claims;

			//get resident id from token
			string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
			string residentId = claimName[(claimName.LastIndexOf(':') + 2)..];

			//get role from token
			string claimRole = claim.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().ToString();
			string role = claimRole.Substring(claimRole.LastIndexOf(':') + 2);

			//get Dashboard
			object response = null;
			if (role.Equals(ResidentType.MERCHANT)) {
				response = await _dashboardService.GetDashboardForMerchant(residentId, days);
			}
			else {
				response = await _dashboardService.GetDashboardForMarketManager(residentId, role, days);
			}

			string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

			watch.Stop();

			_logger.Information($"GET api/dashboard?days={days}" +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}
	}
}