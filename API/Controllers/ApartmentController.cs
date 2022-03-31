using BLL.Dtos;
using BLL.Dtos.Apartment;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/apartments")]
    public class ApartmentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IApartmentService _apartmentService;
        private readonly ITokenService _tokenService;

        public ApartmentController(ILogger logger,
            IApartmentService apartmentService, ITokenService tokenService)
        {
            _logger = logger;
            _apartmentService = apartmentService;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Create Apartment (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] ApartmentRequest apartmentRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"POST api/apartments START Request: " +
                $"{JsonSerializer.Serialize(apartmentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Apartment
            ApartmentResponse response = await _apartmentService.CreateApartment(apartmentRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ApartmentResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/apartments END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Apartment
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetApartment(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string include)
        {
            _logger.Information($"GET api/apartments" +
                $"?id={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort}&include={include} START");

            Stopwatch watch = new();
            watch.Start();

            //get Apartment
            object responses = await _apartmentService.GetApartments(id, status, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(responses));

            watch.Stop();

            _logger.Information($"GET api/apartments" +
                $"id={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort}&include={include} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Apartment (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut]
        public async Task<IActionResult> UpdateApartmentById([FromQuery] string id,
                                              [FromBody] ApartmentRequest apartmentRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/apartments?id={id} START Request: " +
                $"{JsonSerializer.Serialize(apartmentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update Apartment
            ApartmentResponse response = await _apartmentService.UpdateApartmentById(id, apartmentRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ApartmentResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/apartments?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete apartment (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeleteApartment([FromQuery] string id)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"DELETE api/apartment?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete Apartment
            await _apartmentService.DeleteApartment(id);

            string json = JsonSerializer.Serialize(ApiResponse<ApartmentResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/apartment?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
