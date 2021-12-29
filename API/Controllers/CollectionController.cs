using BLL.Dtos;
using BLL.Dtos.Collection;
using BLL.Dtos.CollectionMapping;
using BLL.Dtos.Product;
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
    [Route("api/collection")]
    public class CollectionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICollectionService _collectionService;

        public CollectionController(ILogger logger,
            ICollectionService collectionService)
        {
            _logger = logger;
            _collectionService = collectionService;
        }

        /// <summary>
        /// Create Collection
        /// </summary>
        /// <param name="collectionRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCollection([FromBody] CollectionRequest collectionRequest)
        {
            _logger.Information($"POST api/collection/create START Request: " +
                $"{JsonSerializer.Serialize(collectionRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Collection
            BaseResponse<CollectionResponse> response = await _collectionService.CreateCollection(collectionRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/collection/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Collection By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionById(string id)
        {
            _logger.Information($"GET api/collection/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get Collection
            BaseResponse<CollectionResponse> response = await _collectionService.GetCollectionById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/collection/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Collection By Merchant Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("merchant/{merchantId}")]
        public async Task<IActionResult> GetCollectionByMerchantId(string merchantId)
        {
            _logger.Information($"GET api/collection/merchant/{merchantId} START");

            Stopwatch watch = new();
            watch.Start();

            //get Collection
            BaseResponse<List<CollectionResponse>> response = await _collectionService.GetCollectionByMerchantId(merchantId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/collection/merchant/{merchantId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Collection
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollectionById(string id,
                                              [FromBody] CollectionRequest collectionRequest)
        {
            _logger.Information($"PUT api/collection/{id} START Request: " +
                $"{JsonSerializer.Serialize(collectionRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update Collection
            BaseResponse<CollectionResponse> response = await _collectionService.UpdateCollectionById(id, collectionRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/collection/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete collection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteCollection(string id)
        {
            _logger.Information($"PUT api/collection/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete Collection
            BaseResponse<CollectionResponse> response = await _collectionService.DeleteCollection(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/collection/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Add Product To Collection
        /// </summary>
        /// <param name="collectionMappingRequest"></param>
        /// <returns></returns>
        [HttpPost("{collectionId}/add")]
        public async Task<IActionResult> AddProductToCollection(string collectionId, [FromBody] string[] productIds)
        {
            _logger.Information($"POST api/collection/{collectionId}/add START Request: " +
                $"{JsonSerializer.Serialize(productIds)}");

            Stopwatch watch = new();
            watch.Start();

            //add product to Collection
            BaseResponse<List<CollectionMappingResponse>> response = await _collectionService
                .AddProductsToCollection(collectionId, productIds);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"POST api/collection/{collectionId}/add END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Products By Collection Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsByCollectionId(string id)
        {
            _logger.Information($"PUT api/collection/{id}/products START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get products
            BaseResponse<List<BaseProductResponse>> response = await _collectionService.GetProductsByCollectionId(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/collection/{id}/products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product Status In Collection
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("{collectionId}/update/{productId}/{status}")]
        public async Task<IActionResult> UpdateProductStatusInCollection(
            string collectionId, string productId, int status)
        {
            _logger.Information($"PUT api/collection/{collectionId}/update/{productId}/{status} START Request: " +
                $"{JsonSerializer.Serialize(status)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update product status in collection
            BaseResponse<CollectionMappingResponse> response = 
                await _collectionService.UpdateProductStatusInCollection(collectionId, productId, status);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/collection/{collectionId}/update/{productId}/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Remove Product From Collection
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{collectionId}/delete/{productId}")]
        public async Task<IActionResult> RemoveProductFromCollection(string collectionId, string productId)
        {
            _logger.Information($"PUT api/collection/{collectionId}/delete/{productId} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update product status in collection
            BaseResponse<CollectionMappingResponse> response =
                await _collectionService.RemoveProductFromCollection(collectionId, productId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/collection/{collectionId}/delete/{productId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
