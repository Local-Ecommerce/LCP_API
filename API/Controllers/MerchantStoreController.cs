using BLL.Dtos;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.StoreMenuDetail;
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
    [Route("api/store")]
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
            _logger.Information($"POST api/store/create START Request: " +
                $"{JsonSerializer.Serialize(merchantStoreRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.CreateMerchantStore(merchantStoreRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/store/create END duration: " +
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
            _logger.Information($"GET api/store/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.GetMerchantStoreById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/{id} END duration: " +
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
            _logger.Information($"PUT api/store/{id} START Request: " +
                $"{JsonSerializer.Serialize(merchantStoreRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.UpdateMerchantStoreById(id, merchantStoreRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/store/{id} END duration: " +
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
            _logger.Information($"PUT api/store/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.DeleteMerchantStore(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/store/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant Store By Store Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetMerchantStoreByStoreName(string name)
        {
            _logger.Information($"GET api/store/name/{name} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.GetMerchantStoreByStoreName(name);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/name/{name} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant Store By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("merchant/{merchantId}")]
        public async Task<IActionResult> GetMerchantStoreByStoreMerchantId(string merchantId)
        {
            _logger.Information($"GET api/store/merchant/{merchantId} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get MerchantStore
            BaseResponse<List<MerchantStoreResponse>> response =
                await _merchantStoreService.GetMerchantStoreByMerchantId(merchantId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/merchant/{merchantId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant Store By Apartment Id
        /// </summary>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetMerchantStoreByStoreApartmentId(string apartmentId)
        {
            _logger.Information($"GET api/store/apartment/{apartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<List<MerchantStoreResponse>> response =
                await _merchantStoreService.GetMerchantStoreByApartmentId(apartmentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/apartment/{apartmentId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Add Store Menu Details To Merchant Store
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storeMenuDetailRequest"></param>
        /// <returns></returns>
        [HttpPost("{id}/menu")]
        public async Task<IActionResult> AddStoreMenuDetailsToMerchantStore(string id,
            List<StoreMenuDetailRequest> storeMenuDetailRequest)
        {
            _logger.Information($"POST api/store/{id}/menu START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<List<StoreMenuDetailResponse>> response =
                await _merchantStoreService.AddStoreMenuDetailsToMerchantStore(id, storeMenuDetailRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"POST api/store/{id}/menu END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Store Menu Details By Merchant Store Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/menu")]
        public async Task<IActionResult> GetStoreMenuDetailsByMerchantStoreId(string id)
        {
            _logger.Information($"GET api/store/{id}/menu START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<List<StoreMenuDetailResponse>> response =
                await _merchantStoreService.GetStoreMenuDetailsByMerchantStoreId(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/{id}/menu END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Store Menu Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("menu/{id}")]
        public async Task<IActionResult> GetStoreMenuDetailById(string id)
        {
            _logger.Information($"GET api/store/menu/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<StoreMenuDetailResponse> response =
                await _merchantStoreService.GetStoreMenuDetailById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/menu/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Store Menu Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("menu/{id}")]
        public async Task<IActionResult> UpdateStoreMenuDetailById(string id,
            StoreMenuDetailUpdateRequest storeMenuDetailUpdateRequest)
        {
            _logger.Information($"PUT api/store/menu/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<StoreMenuDetailResponse> response =
                await _merchantStoreService.UpdateStoreMenuDetailById(id, storeMenuDetailUpdateRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/store/menu/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Store Menu Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("menu/delete/{id}")]
        public async Task<IActionResult> DeleteStoreMenuDetailById(string id)
        {
            _logger.Information($"PUT api/store/menu/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<StoreMenuDetailResponse> response =
                await _merchantStoreService.DeleteStoreMenuDetailById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/store/menu/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
