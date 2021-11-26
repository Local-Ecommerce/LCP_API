using BLL.Dtos;
using BLL.Dtos.MerchantStore;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/merchantStore")]
    public class MerchantStoreController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMerchantStoreService _merchantStoreService;

        public MerchantStoreController(ILogger logger,
            IMerchantStoreService merchantStoreService)
        {
            _logger = logger;
            _merchantStoreService = merchantStoreService;
        }

        /// <summary>
        /// Create a Merchant Store
        /// </summary>
        /// <param name="merchantStoreRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateMerchantStore([FromBody] MerchantStoreRequest merchantStoreRequest)
        {
            _logger.Information($"POST api/merchantStore/create START Request: " +
                $"{JsonSerializer.Serialize(merchantStoreRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.CreateMerchantStore(merchantStoreRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/merchantStore/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Merchant Store By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMerchantStoreById(string id)
        {
            _logger.Information($"GET api/merchantStore/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.GetMerchantStoreById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/merchantStore/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MerchantStoreRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMerchantStore(string id,
                                                      [FromBody] MerchantStoreRequest merchantStoreRequest)
        {
            _logger.Information($"PUT api/merchantStore/{id} START Request: " +
                $"{JsonSerializer.Serialize(merchantStoreRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.UpdateMerchantStoreById(id, merchantStoreRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/merchantStore/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteMerchantStore(string id)
        {
            _logger.Information($"PUT api/merchantStore/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.DeleteMerchantStore(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/merchantStore/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
