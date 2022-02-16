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
    [Route("api/combination")]
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
            _logger.Information($"POST api/combination START Request: " +
                $"{JsonSerializer.Serialize(productCombinationRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Product Combination
            ProductCombinationResponse response = await _ProductCombinationService.CreateProductCombination(productCombinationRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCombinationResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/combination END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Product Combination By Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCombinationById(string id)
        {
            _logger.Information($"GET api/combination/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get ProductCombination
            ProductCombinationResponse response = await _ProductCombinationService.GetProductCombinationById(id);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCombinationResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/combination/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product Combination (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCombination(string id,
                                                      [FromBody] ProductCombinationRequest request)
        {
            _logger.Information($"PUT api/combination/{id} START Request: " +
                $"{JsonSerializer.Serialize(request)}");

            Stopwatch watch = new();
            watch.Start();

            //update ProductCombination
            ProductCombinationResponse response = await _ProductCombinationService.UpdateProductCombinationById(id, request);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCombinationResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/combination/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product Combination (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCombination(string id)
        {
            _logger.Information($"PUT api/combination/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete ProductCombination
            ProductCombinationResponse response = await _ProductCombinationService.DeleteProductCombinationById(id);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCombinationResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/combination/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product Combinations By Status
        /// </summary>
        [AllowAnonymous]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetProductCombinationByStatus(int status)
        {
            _logger.Information($"GET api/combination/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get Product Combination
            List<ProductCombinationResponse> response =
                await _ProductCombinationService.GetProductCombinationsByStatus(status);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/combination/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
        
        
        /// <summary>
        /// Get Product Combinations By Product Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProductCombinationByProductId(string id)
        {
            _logger.Information($"GET api/combination/products/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get Product Combination
            List<ProductCombinationResponse> response =
                await _ProductCombinationService.GetProductCombinationsByProductId(id);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/combination/products/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
        
        
        /// <summary>
        /// Get Product Combinations By Base Product Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("base/{id}")]
        public async Task<IActionResult> GetProductCombinationByBaseProductId(string id)
        {
            _logger.Information($"GET api/combination/base/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get Product Combination
            List<ProductCombinationResponse> response =
                await _ProductCombinationService.GetProductCombinationsByBaseProductId(id);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/combination/status/base/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
