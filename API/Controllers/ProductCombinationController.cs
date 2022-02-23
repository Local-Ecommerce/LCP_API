using BLL.Dtos;
using BLL.Dtos.ProductCombination;
using BLL.Services.Interfaces;
using DAL.Constants;
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
    [Route("api/combination-products")]
    public class ProductCombinationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductCombinationService _ProductCombinationService;

        public ProductCombinationController(ILogger logger,
            IProductCombinationService ProductCombinationService)
        {
            _logger = logger;
            _ProductCombinationService = ProductCombinationService;
        }

        /// <summary>
        /// Create a Product Combination (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateProductCombination([FromBody] ProductCombinationRequest productCombinationRequest)
        {
            _logger.Information($"POST api/combination-products START Request: " +
                $"{JsonSerializer.Serialize(productCombinationRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Product Combination
            ProductCombinationResponse response = await _ProductCombinationService.CreateProductCombination(productCombinationRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCombinationResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/combination-products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Product Combination
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProductCombination(
            [FromQuery] string id,
            [FromQuery] string productid,
            [FromQuery] int?[] status,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string include)
        {
            _logger.Information($"GET api/combination-products" +
                $"?id={id}&productid={productid}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort}&include={include} START");

            Stopwatch watch = new();
            watch.Start();

            //get ProductCombination
            object response = await _ProductCombinationService.GetProductCombination(id, productid, status, limit, page, sort);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/combination-products" +
                $"?id={id}&productid={productid}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort}&include={include} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product Combination (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateProductCombination([FromQuery] string id,
                                                      [FromBody] ProductCombinationRequest request)
        {
            _logger.Information($"PUT api/combination-products?id={id} START Request: " +
                $"{JsonSerializer.Serialize(request)}");

            Stopwatch watch = new();
            watch.Start();

            //update ProductCombination
            ProductCombinationResponse response = await _ProductCombinationService.UpdateProductCombinationById(id, request);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCombinationResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/combination-products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product Combination (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductCombination([FromQuery] string id)
        {
            _logger.Information($"PUT api/combination-products?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete ProductCombination
            ProductCombinationResponse response = await _ProductCombinationService.DeleteProductCombinationById(id);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCombinationResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/combination-products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
