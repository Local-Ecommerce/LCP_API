using BLL.Dtos;
using BLL.Dtos.DeliveryAddress;
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
    [Route("api/deliveryAddress")]
    public class DeliveryAddressController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IDeliveryAddressService _deliveryAddressService;

        public DeliveryAddressController(ILogger logger,
            IDeliveryAddressService deliveryAddressService)
        {
            _logger = logger;
            _deliveryAddressService = deliveryAddressService;
        }


        /// <summary>
        /// Create DeliveryAddress
        /// </summary>
        /// <param name="deliveryAddressRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateDeliveryAddress([FromBody] DeliveryAddressRequest deliveryAddressRequest)
        {
            _logger.Information($"POST api/DeliveryAddress/create START Request: " +
                $"{JsonSerializer.Serialize(deliveryAddressRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create DeliveryAddress
            BaseResponse<DeliveryAddressResponse> response = await _deliveryAddressService.CreateDeliveryAddress(deliveryAddressRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/deliveryAddress/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get DeliveryAddress By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryAddressById(string id)
        {
            _logger.Information($"GET api/deliveryAddress/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get DeliveryAddress
            BaseResponse<DeliveryAddressResponse> response = await _deliveryAddressService.GetDeliveryAddressById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/deliveryAddress/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update DeliveryAddress
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryAddressRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeliveryAddressById(string id,
                                              [FromBody] DeliveryAddressRequest deliveryAddressRequest)
        {
            _logger.Information($"PUT api/DeliveryAddress/{id} START Request: " +
                $"{JsonSerializer.Serialize(deliveryAddressRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update DeliveryAddress
            BaseResponse<DeliveryAddressResponse> response = await _deliveryAddressService.UpdateDeliveryAddressById(id, deliveryAddressRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/deliveryAddress/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete DeliveryAddress
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDeliveryAddress(string id)
        {
            _logger.Information($"PUT api/deliveryAddress/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete DeliveryAddress
            BaseResponse<DeliveryAddressResponse> response = await _deliveryAddressService.DeleteDeliveryAddress(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/deliveryAddress/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
