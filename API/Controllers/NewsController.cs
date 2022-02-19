using BLL.Dtos;
using BLL.Dtos.News;
using BLL.Services.Interfaces;
using DAL.Constants;
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
    [Route("api/news")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly INewsService _newsService;

        public NewsController(ILogger logger, INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        /// <summary>
        /// Create news (Admin, Maket Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreateNews([FromBody] NewsRequest newsRequest)
        {
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
        /// Get news by id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(string id)
        {
            _logger.Information($"GET api/news/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get News
            ExtendNewsResponse response = await _newsService.GetNewsById(id);

            string json = JsonSerializer.Serialize(ApiResponse<NewsResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/news/{id} END duration: " + 
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get all news
        /// </summary>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNews()
        {
            _logger.Information($"GET api/news/all START");

            Stopwatch watch = new();
            watch.Start();

            //get News
            List<ExtendNewsResponse> response = await _newsService.GetAllNews();

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/news/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update news (Admin, Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNewsById(string id, [FromBody] NewsUpdateRequest newsRequest)
        {
            _logger.Information($"PUT api/news/{id} START Request: " +
                $"{JsonSerializer.Serialize(newsRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update News
            NewsResponse response = await _newsService.UpdateNewsById(id, newsRequest);

            string json = JsonSerializer.Serialize(ApiResponse<NewsResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/news/{id} END duration: " + 
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Delete news (Admin, Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsById(string id)
        {
            _logger.Information($"DELETE api/news/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete News
            NewsResponse response = await _newsService.DeleteNewsById(id);

            string json = JsonSerializer.Serialize(ApiResponse<NewsResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/news/{id} END duration: " + 
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get news by apartment id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetNewsByApartmentId(string apartmentId)
        {
            _logger.Information($"GET api/news/apartment/{apartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get News by ApartmentId
            List<ExtendNewsResponse> response = await _newsService.GetNewsByAparmentId(apartmentId);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/news/apartment/{apartmentId} END duration: " + 
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get News By Release Date
        /// </summary>
        [AllowAnonymous]
        [HttpGet("bydate/{date}")]
        public async Task<IActionResult> GetNewsByReleaseDate(DateTime date)
        {
            _logger.Information($"GET api/news/{date} START");

            Stopwatch watch = new();
            watch.Start();

            //Get News by RealeaseDate
            List<ExtendNewsResponse> response = await _newsService.GetNewsByReleaseDate(date);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/news/{date} END duration: " + 
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get News By Status (Admin, Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetNewsByStatus(int status)
        {
            _logger.Information($"GET api/news/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get News
            List<ExtendNewsResponse> response =
                await _newsService.GetNewsByStatus(status);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/news/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
