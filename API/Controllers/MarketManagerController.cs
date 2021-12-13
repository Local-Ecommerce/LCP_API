using BLL.Dtos;
using BLL.Dtos.MarketManager;
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
    [Route("api/marketManager")]
    public class MarketManagerController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMarketManagerService _marketManagerService;

        public MarketManagerController(ILogger logger,
            IMarketManagerService marketManagerService)
        {
            _logger = logger;
            _marketManagerService = marketManagerService;
        }


        /// <summary>
        /// Create MarketManager
        /// </summary>
        /// <param name="marketManagerRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateMarketManager([FromBody] MarketManagerRequest marketManagerRequest)
        {
            _logger.Information($"POST api/MarketManager/create START Request: " +
                $"{JsonSerializer.Serialize(marketManagerRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create MarketManager
            BaseResponse<MarketManagerResponse> response = await _marketManagerService.CreateMarketManager(marketManagerRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/MarketManager/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get MarketManager By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketManagerById(string id)
        {
            _logger.Information($"GET api/MarketManager/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get MarketManager
            BaseResponse<MarketManagerResponse> response = await _marketManagerService.GetMarketManagerById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/MarketManager/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update MarketManager
        /// </summary>
        /// <param name="id"></param>
        /// <param name="marketManagerRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMarketManagerById(string id,
                                              [FromBody] MarketManagerRequest marketManagerRequest)
        {
            _logger.Information($"PUT api/MarketManager/{id} START Request: " +
                $"{JsonSerializer.Serialize(marketManagerRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update MarketManager
            BaseResponse<MarketManagerResponse> response = await _marketManagerService.UpdateMarketManagerById(id, marketManagerRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/MarketManager/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete MarketManager
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMarketManager(string id)
        {
            _logger.Information($"PUT api/MarketManager/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete MarketManager
            BaseResponse<MarketManagerResponse> response = await _marketManagerService.DeleteMarketManager(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/MarketManager/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get MarketManager By Account Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetMarketManagerByAccountId(string accountId)
        {
            _logger.Information($"GET api/MarketManager/account/{accountId} START");

            Stopwatch watch = new();
            watch.Start();

            //get MarketManager
            BaseResponse<List<MarketManagerResponse>> response = await _marketManagerService.GetMarketManagerByAccountId(accountId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/MarketManager/merchant/{accountId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get MarketManager By Appartment Id
        /// </summary>
        /// <param name="appartmentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("appartment/{appartmentId}")]
        public async Task<IActionResult> GetMarketManagerByAppartmentId(string appartmentId)
        {
            _logger.Information($"GET api/MarketManager/account/{appartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //get MarketManager
            BaseResponse<List<MarketManagerResponse>> response = await _marketManagerService.GetMarketManagerByAppartmentId(appartmentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/MarketManager/merchant/{appartmentId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
