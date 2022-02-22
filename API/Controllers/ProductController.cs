using BLL.Dtos;
using BLL.Dtos.Product;
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
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// Create Product (base include related product) (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateBaseProduct([FromBody] BaseProductRequest productRequest)
        {
            _logger.Information($"POST api/products START Request: {JsonSerializer.Serialize(productRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create product
            ExtendProductResponse response = await _productService.CreateProduct(productRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Add Related Product (Merchant)
        /// </summary>
        [HttpPost("{id}/related")]
        [Authorize(Roles = ResidentType.MERCHANT)]
        [AllowAnonymous]
        public async Task<IActionResult> AddRelatedProduct(string id, [FromBody] List<ProductRequest> relatedProductRequest)
        {
            _logger.Information($"POST api/products/related/{id} START Request: {JsonSerializer.Serialize(relatedProductRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create product
            ProductResponse response = await _productService.AddRelatedProduct(id, relatedProductRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"POST api/products/related/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProduct(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string include)
        {
            _logger.Information($"GET api/products" +
                $"?id={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort}&include={include} START");

            Stopwatch watch = new();
            watch.Start();

            //get base product
            object response = await _productService.GetProduct(id, status, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/products" +
                $"?id={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort}&include={include} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Request Update Product (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> RequestUpdateProduct([FromQuery] string id,
            [FromBody] ProductRequest productRequest)
        {
            _logger.Information($"PUT api/products?id={id} START Request: {JsonSerializer.Serialize(productRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update product
            ExtendProductResponse response = await _productService.RequestUpdateProduct(id, productRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product by Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] string id)
        {
            _logger.Information($"DELETE api/products?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete product
            ExtendProductResponse response = await _productService.DeleteProduct(id);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Approve  Product With ID (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut("approval")]
        public async Task<IActionResult> ApproveProduct([FromQuery] string id)
        {
            _logger.Information($"PUT api/products/approval?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //approve Product
            ProductResponse response = await _productService.VerifyProductById(id, true);

            string json = JsonSerializer.Serialize(ApiResponse<ProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/products/approval?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Reject Product With ID (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut("rejection")]
        public async Task<IActionResult> RejectCreateProduct([FromQuery] string id)
        {
            _logger.Information($"PUT api/products/rejection?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //reject Product
            ProductResponse response = await _productService.VerifyProductById(id, false);

            string json = JsonSerializer.Serialize(ApiResponse<ProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/products/rejection?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
