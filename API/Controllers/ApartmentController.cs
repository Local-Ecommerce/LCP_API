using BLL.Dtos;
using BLL.Dtos.Apartment;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/apartment")]
    public class ApartmentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IApartmentService _apartmentService;

        public ApartmentController(ILogger logger,
            IApartmentService apartmentService)
        {
            _logger = logger;
            _apartmentService = apartmentService;
        }


        /// <summary>
        /// Create Apartment (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] ApartmentRequest apartmentRequest)
        {
            _logger.Information($"POST api/apartment START Request: " +
                $"{JsonSerializer.Serialize(apartmentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Apartment
            BaseResponse<ApartmentResponse> response = await _apartmentService.CreateApartment(apartmentRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/apartment END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Apartment By Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartmentById(string id)
        {
            _logger.Information($"GET api/apartment/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get Apartment
            BaseResponse<ApartmentResponse> response = await _apartmentService.GetApartmentById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/apartment/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Apartment (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApartmentById(string id,
                                              [FromBody] ApartmentRequest apartmentRequest)
        {
            _logger.Information($"PUT api/apartment/{id} START Request: " +
                $"{JsonSerializer.Serialize(apartmentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update Apartment
            BaseResponse<ApartmentResponse> response = await _apartmentService.UpdateApartmentById(id, apartmentRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/apartment/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete apartment (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(string id)
        {
            _logger.Information($"DELETE api/apartment/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete Apartment
            BaseResponse<ApartmentResponse> response = await _apartmentService.DeleteApartment(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"DELETE api/apartment/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get Apartment By Address
        /// </summary>
        [AllowAnonymous]
        [HttpGet("address/{address}")]
        public async Task<IActionResult> GetApartmentByAddress(string address)
        {
            _logger.Information($"GET api/apartment/address/{address} START");

            Stopwatch watch = new();
            watch.Start();

            //get Apartment
            BaseResponse<ApartmentResponse> response = await _apartmentService.GetApartmentByAddress(address);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/apartment/address/{address} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Apartments By Status (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetApartmentsByStatus(int status)
        {
            _logger.Information($"GET api/apartment/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get Apartment
            BaseResponse<List<ApartmentResponse>> responses = await _apartmentService.GetApartmentsByStatus(status);

            string json = JsonSerializer.Serialize(responses);

            watch.Stop();

            _logger.Information($"GET api/apartment/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Apartments For Auto Complete
        /// </summary>
        [AllowAnonymous]
        [HttpGet("autocomplete")]
        public async Task<IActionResult> GetApartmentsForAutoComplete()
        {
            _logger.Information($"GET api/apartment/autocomplete START");

            Stopwatch watch = new();
            watch.Start();

            //get Apartment
            BaseResponse<List<ApartmentResponse>> responses = await _apartmentService.GetApartmentForAutoComplete();

            string json = JsonSerializer.Serialize(responses);

            watch.Stop();

            _logger.Information($"GET api/apartment/autocomplete END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get All Apartments
        /// </summary>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllApartments()
        {
            _logger.Information($"GET api/apartment/all START");

            Stopwatch watch = new();
            watch.Start();

            //get Apartment
            BaseResponse<List<ApartmentResponse>> responses = await _apartmentService.GetAllApartments();

            string json = JsonSerializer.Serialize(responses);

            watch.Stop();

            _logger.Information($"GET api/apartment/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Market Manager By Apartment Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("resident/{apartmentId}")]
        public async Task<IActionResult> GetMarketManagerByApartmentId(string apartmentId)
        {
            _logger.Information($"GET api/apartment/{apartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //get Apartment
            BaseResponse<ExtendApartmentResponse> response = await _apartmentService.GetMarketManagerByApartmentId(apartmentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/apartment/{apartmentId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
