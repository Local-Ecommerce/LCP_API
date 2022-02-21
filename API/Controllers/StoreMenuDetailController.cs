using System.Collections.Generic;
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
    [Route("api/store-menus")]
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
        /// Add Store Menu Details To Merchant Store (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateStoreMenuDetail([FromQuery] string storeid, [FromBody] List<StoreMenuDetailRequest> requests)
        {
            _logger.Information($"POST api/store-menus?storeid={storeid} START Request: " +
                $"{JsonSerializer.Serialize(requests)}");

            Stopwatch watch = new();
            watch.Start();

            //create
            List<StoreMenuDetailResponse> response = await _storeMenuDetailService.AddStoreMenuDetailsToMerchantStore(storeid, requests);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"POST api/store-menus?storeid={storeid} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Store Menu Detail By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateStoreMenuDetailById([FromQuery] string id, [FromBody] StoreMenuDetailUpdateRequest request)
        {
            _logger.Information($"PUT api/store-menus?id={id} START Request: " +
                $"{JsonSerializer.Serialize(request)}");

            Stopwatch watch = new();
            watch.Start();

            //update
            StoreMenuDetailResponse response = 
                await _storeMenuDetailService.UpdateStoreMenuDetailById(id, request);

            string json = JsonSerializer.Serialize(ApiResponse<StoreMenuDetailResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/store-menus?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Store Menu Detail By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteStoreMenuDetailById([FromQuery] string id)
        {
            _logger.Information($"DELETE api/store-menus?id={id} START Request: ");

            Stopwatch watch = new();
            watch.Start();

            //delete
            StoreMenuDetailResponse response =
                await _storeMenuDetailService.DeleteStoreMenuDetailById(id);

            string json = JsonSerializer.Serialize(ApiResponse<StoreMenuDetailResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/store-menus?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}