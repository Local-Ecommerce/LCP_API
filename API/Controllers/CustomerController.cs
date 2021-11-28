using BLL.Dtos;
using BLL.Dtos.Customer;
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
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger logger,
            ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }


        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequest customerRequest)
        {
            _logger.Information($"POST api/customer/create START Request: " +
                $"{JsonSerializer.Serialize(customerRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Customer
            BaseResponse<CustomerResponse> response = await _customerService.CreateCustomer(customerRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/customer/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(string id)
        {
            _logger.Information($"GET api/customer/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<CustomerResponse> response = await _customerService.GetCustomerById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerById(string id,
                                              [FromBody] CustomerRequest customerRequest)
        {
            _logger.Information($"PUT api/customer/{id} START Request: " +
                $"{JsonSerializer.Serialize(customerRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update Customer
            BaseResponse<CustomerResponse> response = await _customerService.UpdateCustomerById(id, customerRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/customer/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            _logger.Information($"PUT api/customer/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete Customer
            BaseResponse<CustomerResponse> response = await _customerService.DeleteCustomer(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/customer/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
