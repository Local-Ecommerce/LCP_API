using BLL.Dtos;
using BLL.Dtos.New;
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
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateNews([FromBody] NewsRequest newsRequest)
        {
            _logger.Information($"POST api/news/create START Request: " + $"{JsonSerializer.Serialize(newsRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create News
            BaseResponse<NewsResponse> response = await _newsService.CreateNews(newsRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/news/create END duration: " + $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
        
        /// <summary>
        /// Get news by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(string id)
        {
            _logger.Information($"GET api/news/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get News
            BaseResponse<NewsResponse> response = await _newsService.GetNewsById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/news/{id} END duration: " + $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update news
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateNewsById(string id, [FromBody] NewsRequest newsRequest)
        {
            _logger.Information($"PUT api/news/update/{id} START Request: " + $"{JsonSerializer.Serialize(newsRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update News
            BaseResponse<NewsResponse> response = await _newsService.UpdateNewsById(id, newsRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/news/update/{id} END duration: " + $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Delete news
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteNewsById(string id)
        {
            _logger.Information($"PUT api/news/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete News
            BaseResponse<NewsResponse> response = await _newsService.DeleteNewsById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/news/delete/{id} END duration: " + $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get news by apartmentid
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetNewsByApartmentId(string apartmentId)
        {
            _logger.Information($"GET api/news/apartment/{apartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get News by ApartmentId
            BaseResponse<List<NewsResponse>> response = await _newsService.GetNewsByAparmentId(apartmentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/news/apartment/{apartmentId} END duration: " + $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get news by marketmanagerId
        /// </summary>
        /// <param name="marketManagerId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("marketmanager/{marketManagerId}")]
        public async Task<IActionResult> GetNewsByMarketManagerId(string marketManagerId)
        {
            _logger.Information($"GET api/news/marketmanager/{marketManagerId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get News by MarketManager
            BaseResponse<List<NewsResponse>> response = await _newsService.GetNewsByMarketManagerId(marketManagerId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/news/marketmanager/{marketManagerId} END duration: " + $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Not working as intended, fix later
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("bydate/{date}")]
        public async Task<IActionResult> GetNewsByReleaseDate(DateTime date)
        {
            _logger.Information($"GET api/news/{date} START");

            Stopwatch watch = new();
            watch.Start();

            //Get News by RealeaseDate
            BaseResponse<List<NewsResponse>> response = await _newsService.GetNewsByReleaseDate(date);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/news/{date} END duration: " + $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
