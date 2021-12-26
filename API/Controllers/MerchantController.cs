using BLL.Dtos;
using BLL.Dtos.Merchant;
using BLL.Services.Interfaces;
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
    [Route("api/merchant")]
    public class MerchantController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMerchantService _merchantService;

        public MerchantController(ILogger logger,
            IMerchantService merchantService)
        {
            _logger = logger;
            _merchantService = merchantService;
        }

        /// <summary>
        /// Create a Merchant
        /// </summary>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateMerchant([FromBody] MerchantRequest merchantRequest)
        {
            _logger.Information($"POST api/merchant/create START Request: " +
                $"{JsonSerializer.Serialize(merchantRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //create Merchant
            BaseResponse<MerchantResponse> response = await _merchantService.CreateMerchant(merchantRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/merchant/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Merchant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMerchantById(string id)
        {
            _logger.Information($"GET api/merchant/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get merchant
            BaseResponse<MerchantResponse> response = await _merchantService.GetMerchantById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/merchant/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMerchant(string id,
                                                      [FromBody] MerchantRequest merchantRequest)
        {
            _logger.Information($"PUT api/merchant/{id} START Request: " +
                $"{JsonSerializer.Serialize(merchantRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update merchant
            BaseResponse<MerchantResponse> response = await _merchantService.UpdateMerchantById(id, merchantRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/merchant/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteMerchant(string id)
        {
            _logger.Information($"PUT api/merchant/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete merchant
            BaseResponse<MerchantResponse> response = await _merchantService.DeleteMerchant(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/merchant/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetMerchantByName(string name)
        {
            _logger.Information($"GET api/merchant/name/{name} START");

            Stopwatch watch = new();
            watch.Start();

            //get merchant
            BaseResponse<MerchantResponse> response = await _merchantService.GetMerchantByName(name);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/merchant/{name} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant By Address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("address/{address}")]
        public async Task<IActionResult> GetMerchantByAddress(string address)
        {
            _logger.Information($"GET api/merchant/address/{address} START");

            Stopwatch watch = new();
            watch.Start();

            //get merchant
            BaseResponse<MerchantResponse> response = await _merchantService.GetMerchantByAddress(address);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/merchant/address/{address} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant By Phone Number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("number/{number}")]
        public async Task<IActionResult> GetMerchantByPhoneNumber(string number)
        {
            _logger.Information($"GET api/merchant/number/{number} START");

            Stopwatch watch = new();
            watch.Start();

            //get merchant
            BaseResponse<MerchantResponse> response = await _merchantService.GetMerchantByPhoneNumber(number);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/merchant/number/{number} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant By AccountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetMerchantByAccountId(string accountId)
        {
            _logger.Information($"GET api/merchant/account/{accountId} START");

            Stopwatch watch = new();
            watch.Start();

            //get merchant
            BaseResponse<List<MerchantResponse>> response = await _merchantService.GetMerchantByAccountId(accountId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/merchant/account/{accountId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
