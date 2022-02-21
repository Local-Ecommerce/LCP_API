using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.POI;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/pois")]
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
        /// Create poi (Admin, Market Manager)
        /// </summary>
        [AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
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
        /// Get poi
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPoi(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] string apartmentid,
            [FromQuery] DateTime date,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string[] include)
        {
            _logger.Information($"GET api/poi?id={id}&status=" + string.Join("status=", status) +
                $"&apartmentid={apartmentid}&date={date}&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) +
                $"START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi
            object response = await _poiService.GetPoi(id, apartmentid, date, status, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/poi?id={id}&status=" + string.Join("status=", status) +
                $"&apartmentid={apartmentid}&date={date}&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) +
                $"END duration: {watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Poi (Admin, Market Manager)
        /// </summary>
        [AuthorizeRoles(ResidentType.MARKET_MANAGER, RoleId.ADMIN)]
        [HttpPut]
        public async Task<IActionResult> UpdatePoiById([FromQuery]string id, [FromBody] PoiUpdateRequest poiRequest)
        {
            _logger.Information($"PUT api/poi?id={id} START Request: " +
                $"{JsonSerializer.Serialize(poiRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update Poi
            PoiResponse response = await _poiService.UpdatePoiById(id, poiRequest);

            string json = JsonSerializer.Serialize(ApiResponse<PoiResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/poi?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete poi (Admin, Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeletePoisById([FromQuery]string id)
        {
            _logger.Information($"DELETE api/poi?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete Poi
            PoiResponse response = await _poiService.DeletePoiById(id);

            string json = JsonSerializer.Serialize(ApiResponse<PoiResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/poi?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
