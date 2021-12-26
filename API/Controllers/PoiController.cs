using BLL.Dtos;
using BLL.Dtos.POI;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/poi")]
    public class PoiController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPoiService _poiService;

        public PoiController(ILogger logger, IPoiService poiService)
        {
            _logger = logger;
            _poiService = poiService;
        }

        /// <summary>
        /// Create poi
        /// </summary>
        /// <param name="poiRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreatePoi([FromBody] PoiRequest poiRequest)
        {
            _logger.Information($"POST api/poi/create START Request: " +
                $"{JsonSerializer.Serialize(poiRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create Poi
            BaseResponse<PoiResponse> response = await _poiService.CreatePoi(poiRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/poi/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get poi by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoiById(string id)
        {
            _logger.Information($"GET api/poi/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi
            BaseResponse<PoiResponse> response = await _poiService.GetPoiById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/poi/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update Poi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsRequest"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePoiById(string id, [FromBody] PoiRequest poiRequest, [FromBody] int status)
        {
            _logger.Information($"PUT api/poi/update/{id} START Request: " +
                $"{JsonSerializer.Serialize(poiRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update Poi
            BaseResponse<PoiResponse> response = await _poiService.UpdatePoiById(id, status, poiRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/poi/update/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Delete poi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeletePoisById(string id)
        {
            _logger.Information($"PUT api/poi/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete Poi
            BaseResponse<PoiResponse> response = await _poiService.DeletePoiById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/poi/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get poi by apartment id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetPoiByApartmentId(string apartmentId)
        {
            _logger.Information($"GET api/poi/apartment/{apartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi by ApartmentId
            BaseResponse<List<PoiResponse>> response = await _poiService.GetPoiByApartmentId(apartmentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/poi/apartment/{apartmentId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get poi by market manager Id
        /// </summary>
        /// <param name="marketManagerId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("marketmanager/{marketManagerId}")]
        public async Task<IActionResult> GetPoiByMarketManagerId(string marketManagerId)
        {
            _logger.Information($"GET api/poi/marketmanager/{marketManagerId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi by MarketManager
            BaseResponse<List<PoiResponse>> response = await _poiService.GetPoiByMarketManagerId(marketManagerId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/poi/marketmanager/{marketManagerId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get Poi By Release Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("bydate/{date}")]
        public async Task<IActionResult> GetPoiByReleaseDate(DateTime date)
        {
            _logger.Information($"GET api/poi/{date} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi by RealeaseDate
            BaseResponse<List<PoiResponse>> response = await _poiService.GetPoiByReleaseDate(date);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/poi/{date} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
