using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.News;
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
	[Route("api/news")]
	public class NewsController : ControllerBase {
		private readonly ILogger _logger;
		private readonly INewsService _newsService;
		private readonly ITokenService _tokenService;

		public NewsController(ILogger logger, INewsService newsService, ITokenService tokenService) {
			_logger = logger;
			_newsService = newsService;
			_tokenService = tokenService;
		}

		/// <summary>
		/// Create news (Admin, Maket Manager)
		/// </summary>
		[AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
		[HttpPost]
		public async Task<IActionResult> CreateNews([FromBody] NewsRequest newsRequest) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"POST api/news START Request: " +
					$"{JsonSerializer.Serialize(newsRequest)}");

			Stopwatch watch = new();
			watch.Start();

			//Create News
			NewsResponse response = await _newsService.CreateNews(newsRequest);

			string json = JsonSerializer.Serialize(ApiResponse<NewsResponse>.Success(response));

			watch.Stop();

			_logger.Information("POST api/news END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}


		/// <summary>
		/// Get news
		/// </summary>
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetNews(
				[FromQuery] string id,
				[FromQuery] int?[] status,
				[FromQuery] string apartmentid,
				[FromQuery] string type,
				[FromQuery] DateTime? date,
				[FromQuery] string search,
				[FromQuery] int? limit,
				[FromQuery] int? page,
				[FromQuery] string[] sort,
				[FromQuery] string[] include) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"GET api/news?id={id}&status=" + string.Join("status=", status) +
					$"&apartmentid={apartmentid}&type={type}&date={date}&search={search}" +
					$"&limit={limit}&page={page}&sort=" + string.Join("sort=", sort) +
					$"&include={include} START");

			Stopwatch watch = new();
			watch.Start();

			//Get News
			object response = await _newsService.GetNews(id, apartmentid, type, date, search, status, limit, page, sort, include);

			string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

			watch.Stop();

			_logger.Information($"GET api/news?id={id}&status=" + string.Join("status=", status) +
					$"&apartmentid={apartmentid}&type={type}&date={date}&search={search}" +
					$"&limit={limit}&page={page}&sort=" + string.Join("sort=", sort) +
					$"&include={include} END duration: {watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}

		/// <summary>
		/// Update news (Admin, Market Manager)
		/// </summary>
		[AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
		[HttpPut]
		public async Task<IActionResult> UpdateNewsById([FromQuery] string id, [FromBody] NewsUpdateRequest newsRequest) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"PUT api/news?id={id} START Request: " +
					$"{JsonSerializer.Serialize(newsRequest)}");

			Stopwatch watch = new();
			watch.Start();

			//Update News
			NewsResponse response = await _newsService.UpdateNewsById(id, newsRequest);

			string json = JsonSerializer.Serialize(ApiResponse<NewsResponse>.Success(response));

			watch.Stop();

			_logger.Information($"PUT api/news?id={id} END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}


		/// <summary>
		/// Delete news (Admin, Market Manager)
		/// </summary>
		[AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
		[HttpDelete]
		public async Task<IActionResult> DeleteNewsById([FromQuery] string id) {
			//check token expired
			_tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

			_logger.Information($"DELETE api/news?id={id} START");

			Stopwatch watch = new();
			watch.Start();

			//Delete News
			await _newsService.DeleteNewsById(id);

			string json = JsonSerializer.Serialize(ApiResponse<NewsResponse>.Success());

			watch.Stop();

			_logger.Information($"DELETE api/news?id={id} END duration: " +
					$"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

			return Ok(json);
		}
	}
}
