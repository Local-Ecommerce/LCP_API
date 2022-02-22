using BLL.Dtos;
using BLL.Dtos.Resident;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/residents")]
    public class ResidentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IResidentService _residentService;

        public ResidentController(ILogger logger,
            IResidentService residentService)
        {
            _logger = logger;
            _residentService = residentService;
        }


        /// <summary>
        /// Create a Resident (Apartment Roles)
        /// </summary>
        [Authorize(Roles = RoleId.APARTMENT)]
        [HttpPost]
        public async Task<IActionResult> CreateResident([FromBody] ResidentRequest residentRequest)
        {
            _logger.Information($"POST api/residents START Request: " +
                $"{JsonSerializer.Serialize(residentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Resident
            ResidentResponse response = await _residentService.CreateResident(residentRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ResidentResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/residents END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Resident (Authentication required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetResident(
            [FromQuery] string id,
            [FromQuery] string apartmentid,
            [FromQuery] string accountid,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort)
        {
            _logger.Information($"GET api/residents?id={id}&apartmentid={apartmentid}&accountid={accountid}" +
                $"&limit={limit}&page={page}&sort={sort} START");

            Stopwatch watch = new ();
            watch.Start();

            //get Resident
            object response = await _residentService.GetResident(id, apartmentid, accountid, limit, page, sort);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/residents?id={id}&apartmentid={apartmentid}&accountid={accountid}" +
                $"&limit={limit}&page={page}&sort={sort} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Resident (Apartment Roles)
        /// </summary>
        [Authorize(Roles = RoleId.APARTMENT)]
        [HttpPut]
        public async Task<IActionResult> UpdateResident([FromQuery]string id,
                                                      [FromBody] ResidentUpdateRequest residentUpdateRequest)
        {
            _logger.Information($"PUT api/residents?id={id} START Request: " +
                $"{JsonSerializer.Serialize(residentUpdateRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update Resident
            ResidentResponse response = await _residentService.UpdateResidentById(id, residentUpdateRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ResidentResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/residents?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Resident (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeleteResident([FromQuery] string id)
        {
            _logger.Information($"DELETE api/residents?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete Resident
            ResidentResponse response = await _residentService.DeleteResident(id);

            string json = JsonSerializer.Serialize(ApiResponse<ResidentResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/residents?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
