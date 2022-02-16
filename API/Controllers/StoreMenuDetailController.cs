using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using BLL.Dtos;
using BLL.Dtos.StoreMenuDetail;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/storeMenuDetail")]
    public class StoreMenuDetailController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IStoreMenuDetailService _storeMenuDetailService;

        public StoreMenuDetailController(ILogger logger,
            IStoreMenuDetailService storeMenuDetailService)
        {
            _logger = logger;
            _storeMenuDetailService = storeMenuDetailService;
        }

        /// <summary>
        /// Create Store Menu Detail (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateStoreMenuDetail([FromBody] StoreMenuDetailRequest request)
        {
            _logger.Information($"POST api/storeMenuDetail START Request: " +
                $"{JsonSerializer.Serialize(request)}");

            Stopwatch watch = new();
            watch.Start();

            //create
            StoreMenuDetailResponse response = await _storeMenuDetailService.CreateStoreMenuDetail(request);

            string json = JsonSerializer.Serialize(ApiResponse<StoreMenuDetailResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/storeMenuDetail END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Store Menu Detail By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStoreMenuDetailById(string id, [FromBody] StoreMenuDetailUpdateRequest request)
        {
            _logger.Information($"PUT api/storeMenuDetail/{id} START Request: " +
                $"{JsonSerializer.Serialize(request)}");

            Stopwatch watch = new();
            watch.Start();

            //update
            StoreMenuDetailResponse response = 
                await _storeMenuDetailService.UpdateStoreMenuDetailById(id, request);

            string json = JsonSerializer.Serialize(ApiResponse<StoreMenuDetailResponse>.Success(response));

            watch.Stop();

            _logger.Information("PUT api/storeMenuDetail/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Store Menu Detail By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoreMenuDetailById(string id)
        {
            _logger.Information($"PUT api/storeMenuDetail/{id} START Request: ");

            Stopwatch watch = new();
            watch.Start();

            //delete
            StoreMenuDetailResponse response =
                await _storeMenuDetailService.DeleteStoreMenuDetailById(id);

            string json = JsonSerializer.Serialize(ApiResponse<StoreMenuDetailResponse>.Success(response));

            watch.Stop();

            _logger.Information("PUT api/storeMenuDetail/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}