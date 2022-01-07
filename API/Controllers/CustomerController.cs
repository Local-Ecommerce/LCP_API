using BLL.Dtos;
using BLL.Dtos.Customer;
using BLL.Services.Interfaces;
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


        /// <summary>
        /// Get Customer By Name
        /// </summary>
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCustomerByName(string name)
        {
            _logger.Information($"GET api/customer/name/{name} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<CustomerResponse> response = await _customerService.GetCustomerByName(name);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/name/{name} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Customer By Phone Number
        /// </summary>
        [AllowAnonymous]
        [HttpGet("phone/{phoneNum}")]
        public async Task<IActionResult> GetCustomerByPhoneNumber(string phoneNum)
        {
            _logger.Information($"GET api/customer/phone/{phoneNum} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<CustomerResponse> response = await _customerService.GetCustomerByPhoneNumber(phoneNum);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/phone/{phoneNum} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Customer By Date Of Birth
        /// </summary>
        [AllowAnonymous]
        [HttpGet("dob/{dob}")]
        public async Task<IActionResult> GetCustomerByDateOfBirth(DateTime dob)
        {
            _logger.Information($"GET api/customer/dob/{dob} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<CustomerResponse> response = await _customerService.GetCustomerByDateOfBirth(dob);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/dob/{dob} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Customer By Created Date
        /// </summary>
        [HttpGet("created/{crDate}")]
        public async Task<IActionResult> GetCustomersByCreatedDate(DateTime crDate)
        {
            _logger.Information($"GET api/customer/created/{crDate} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<List<CustomerResponse>> response = await _customerService.GetCustomersByCreatedDate(crDate);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/created/{crDate} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Customer By Update Date
        /// </summary>
        [AllowAnonymous]
        [HttpGet("updateD/{uDate}")]
        public async Task<IActionResult> GetCustomersByUpdateDate(DateTime uDate)
        {
            _logger.Information($"GET api/customer/updateD/{uDate} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<List<CustomerResponse>> response = await _customerService.GetCustomersByUpdateDate(uDate);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/updateD/{uDate} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Customer By Gender
        /// </summary>
        [AllowAnonymous]
        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetCustomersByGender(string gender)
        {
            _logger.Information($"GET api/customer/gender/{gender} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<List<CustomerResponse>> response = await _customerService.GetCustomersByGender(gender);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/gender/{gender} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Customer By Account Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetCustomersAccountId(string accountId)
        {
            _logger.Information($"GET api/customer/account/{accountId} START");

            Stopwatch watch = new();
            watch.Start();

            //get Customer
            BaseResponse<List<CustomerResponse>> response = await _customerService.GetCustomersByAccountId(accountId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/customer/account/{accountId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
