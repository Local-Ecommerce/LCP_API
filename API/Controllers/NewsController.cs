using BLL.Dtos;
using BLL.Dtos.News;
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
        /// Create news
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateNews([FromBody] NewsRequest newsRequest)
        {
            _logger.Information($"POST api/news START Request: " + 
                $"{JsonSerializer.Serialize(newsRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create News
            BaseResponse<NewsResponse> response = await _newsService.CreateNews(newsRequest);

            string json = JsonSerializer.Serialize(response);

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
            BaseResponse<ExtendNewsResponse> response = await _newsService.GetNewsById(id);

            string json = JsonSerializer.Serialize(response);

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
            BaseResponse<List<ExtendNewsResponse>> response = await _newsService.GetAllNews();

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/news/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update news
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNewsById(string id, [FromBody] NewsUpdateRequest newsRequest)
        {
            _logger.Information($"PUT api/news/{id} START Request: " +
                $"{JsonSerializer.Serialize(newsRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update News
            BaseResponse<NewsResponse> response = await _newsService.UpdateNewsById(id, newsRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/news/{id} END duration: " + 
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Delete news
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsById(string id)
        {
            _logger.Information($"DELETE api/news/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete News
            BaseResponse<NewsResponse> response = await _newsService.DeleteNewsById(id);

            string json = JsonSerializer.Serialize(response);

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
            BaseResponse<List<ExtendNewsResponse>> response = await _newsService.GetNewsByAparmentId(apartmentId);

            string json = JsonSerializer.Serialize(response);

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
            BaseResponse<List<ExtendNewsResponse>> response = await _newsService.GetNewsByReleaseDate(date);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/news/{date} END duration: " + 
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get News By Status
        /// </summary>
        [AllowAnonymous]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetNewsByStatus(int status)
        {
            _logger.Information($"GET api/news/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get News
            BaseResponse<List<ExtendNewsResponse>> response =
                await _newsService.GetNewsByStatus(status);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/news/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
