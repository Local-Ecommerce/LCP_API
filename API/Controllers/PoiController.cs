using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.POI;
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

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/pois")]
    public class PoiController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPoiService _poiService;
        private readonly ITokenService _tokenService;

        public PoiController(ILogger logger, IPoiService poiService, ITokenService tokenService)
        {
            _logger = logger;
            _poiService = poiService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Create poi (Admin, Market Manager)
        /// </summary>
        [AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
        [HttpPost]
        public async Task<IActionResult> CreatePoi([FromBody] PoiRequest poiRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

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
        /// Get poi (Authentication required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPoi(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] string apartmentid,
            [FromQuery] bool? isPriority,
            [FromQuery] string type,
            [FromQuery] DateTime date,
            [FromQuery] string search,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string[] include)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/poi?id={id}&status=" + string.Join("status=", status) +
                $"&apartmentid={apartmentid}&isPriority={isPriority}&type={type}&date={date}&search={search}&limit={limit}&page={page}&sort={sort}&include="
                + string.Join("include=", include) + "START");

            Stopwatch watch = new();
            watch.Start();

            //Get Poi
            object response = await _poiService
                            .GetPois(id, apartmentid, isPriority, type, date, search, status, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/poi?id={id}&status=" + string.Join("status=", status) +
                $"&apartmentid={apartmentid}&isPriority={isPriority}&type={type}&date={date}&search={search}&limit={limit}&page={page}&sort={sort}&include="
                + string.Join("include=", include) + "END duration: {watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Poi (Admin, Market Manager)
        /// </summary>
        [AuthorizeRoles(ResidentType.MARKET_MANAGER, RoleId.ADMIN)]
        [HttpPut]
        public async Task<IActionResult> UpdatePoiById([FromQuery] string id, [FromBody] PoiUpdateRequest poiRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

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
        [AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
        [HttpDelete]
        public async Task<IActionResult> DeletePoisById([FromQuery] string id)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"DELETE api/poi?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete Poi
            await _poiService.DeletePoiById(id);

            string json = JsonSerializer.Serialize(ApiResponse<PoiResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/poi?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
