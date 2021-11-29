using BLL.Dtos;
using BLL.Dtos.Apartment;
using BLL.Services.Interfaces;
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
        /// Create Apartment
        /// </summary>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateApartment([FromBody] ApartmentRequest apartmentRequest)
        {
            _logger.Information($"POST api/apartment/create START Request: " +
                $"{JsonSerializer.Serialize(apartmentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Apartment
            BaseResponse<ApartmentResponse> response = await _apartmentService.CreateApartment(apartmentRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/apartment/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Apartment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Update Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentRequest"></param>
        /// <returns></returns>
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
        /// Delete apartment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteApartment(string id)
        {
            _logger.Information($"PUT api/apartment/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete Apartment
            BaseResponse<ApartmentResponse> response = await _apartmentService.DeleteApartment(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/apartment/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
