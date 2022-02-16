using BLL.Dtos;
using BLL.Dtos.POI;
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
        /// Create poi (Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpPost]
        public async Task<IActionResult> CreatePoi([FromBody] PoiRequest poiRequest)
        {
            _logger.Information($"POST api/poi START Request: " +
                $"{JsonSerializer.Serialize(poiRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create Poi
            PoiResponse response = await _poiService.CreatePoi(poiRequest);

            string json = JsonSerializer.Serialize(ApiResponse<PoiResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/poi END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get poi by id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoiById(string id)
        {
            _logger.Information($"GET api/poi/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi
            ExtendPoiResponse response = await _poiService.GetPoiById(id);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendPoiResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/poi/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get all poi
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPoi()
        {
            _logger.Information($"GET api/poi/all START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi
            List<ExtendPoiResponse> response = await _poiService.GetAllPoi();

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/poi/getall END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update Poi (Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePoiById(string id, [FromBody] PoiUpdateRequest poiRequest)
        {
            _logger.Information($"PUT api/poi/{id} START Request: " +
                $"{JsonSerializer.Serialize(poiRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update Poi
            PoiResponse response = await _poiService.UpdatePoiById(id, poiRequest);

            string json = JsonSerializer.Serialize(ApiResponse<PoiResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/poi/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Delete poi (Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoisById(string id)
        {
            _logger.Information($"PUT api/poi/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete Poi
            PoiResponse response = await _poiService.DeletePoiById(id);

            string json = JsonSerializer.Serialize(ApiResponse<PoiResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/poi/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get poi by apartment id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetPoiByApartmentId(string apartmentId)
        {
            _logger.Information($"GET api/poi/apartment/{apartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi by ApartmentId
            List<ExtendPoiResponse> response = await _poiService.GetPoiByApartmentId(apartmentId);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/poi/apartment/{apartmentId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Poi By Release Date
        /// </summary>
        [AllowAnonymous]
        [HttpGet("bydate/{date}")]
        public async Task<IActionResult> GetPoiByReleaseDate(DateTime date)
        {
            _logger.Information($"GET api/poi/{date} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi by RealeaseDate
            List<ExtendPoiResponse> response = await _poiService.GetPoiByReleaseDate(date);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/poi/{date} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Poi By Status (Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetPoiByStatus(int status)
        {
            _logger.Information($"GET api/poi/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get Poi
            List<ExtendPoiResponse> response = await _poiService.GetPoisByStatus(status);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/poi/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
