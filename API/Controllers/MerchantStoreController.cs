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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMerchantStore(string id,
                                                      [FromBody] MerchantStoreUpdateRequest request)
        {
            _logger.Information($"PUT api/store/{id} START Request: " +
                $"{JsonSerializer.Serialize(request)}");

            Stopwatch watch = new();
            watch.Start();

            //update MerchantStore
            BaseResponse<MerchantStoreResponse> response = await _merchantStoreService.UpdateMerchantStoreById(id, request);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/store/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Merchant Store
        /// </summary>
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
        /// Get Merchant Store By Apartment Id
        /// </summary>
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


        /// <summary>
        /// Get Merchant Stores By Status
        /// </summary>
        [AllowAnonymous]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetMerchantStoreByStatus(int status)
        {
            _logger.Information($"GET api/store/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<List<MerchantStoreResponse>> response =
                await _merchantStoreService.GetMerchantStoresByStatus(status);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get All Merchant Stores
        /// </summary>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMerchantStore()
        {
            _logger.Information($"GET api/store/all START");

            Stopwatch watch = new();
            watch.Start();

            //get MerchantStore
            BaseResponse<List<MerchantStoreResponse>> response =
                await _merchantStoreService.GetAllMerchantStores();

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        
        /// <summary>
        /// Get Pending Merchant Stores
        /// </summary>
        [AllowAnonymous]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingMerchantStores()
        {
            _logger.Information($"GET api/store/pending START");

            Stopwatch watch = new();
            watch.Start();

            //get pending merchantStore
            BaseResponse<List<MerchantStoreResponse>> response =
                await _merchantStoreService.GetPendingMerchantStores();

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/store/pending END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
