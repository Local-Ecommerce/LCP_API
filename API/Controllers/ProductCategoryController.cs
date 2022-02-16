using BLL.Dtos;
using BLL.Dtos.ProductCategory;
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
    [Route("api/productCategory")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(ILogger logger,
            IProductCategoryService productCategoryService)
        {
            _logger = logger;
            _productCategoryService = productCategoryService;
        }

        /// <summary>
        /// Create a Product Category (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateProductCategory([FromBody] ProductCategoryRequest productCategoryRequest)
        {
            _logger.Information($"POST api/productCategory START Request: {JsonSerializer.Serialize(productCategoryRequest)}");

            Stopwatch watch = new ();
            watch.Start();

            //create product Category
            ProductCategoryResponse response = await _productCategoryService.CreateProCategory(productCategoryRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/productCategory END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product Category By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategoryById(string id)
        {
            _logger.Information($"GET api/productCategory/{id} START");

            Stopwatch watch = new ();
            watch.Start();

            //get productCategory
            ExtendProductCategoryResponse response = await _productCategoryService.GetProCategoryById(id);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/productCategory/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product Category (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory(string id,
            [FromBody] ProductCategoryRequest productCategoryRequest)
        {
            _logger.Information($"PUT api/productCategory/{id} START Request: {JsonSerializer.Serialize(productCategoryRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update productCategory
            ProductCategoryResponse response = await _productCategoryService.UpdateProCategory(id, productCategoryRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/productCategory/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete ProductCategory by Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(string id)
        {
            _logger.Information($"DELETE api/productCategory/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete productCategory
            ProductCategoryResponse response = await _productCategoryService.DeleteProCategory(id);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/productCategory/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product Categories By Status
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetProductCategoriesByStatus(int status)
        {
            _logger.Information($"GET api/productCategory/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get Product
            List<ExtendProductCategoryResponse> response =
                await _productCategoryService.GetProductCategoriesByStatus(status);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/productCategory/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
