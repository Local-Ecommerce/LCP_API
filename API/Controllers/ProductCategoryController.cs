using BLL.Dtos;
using BLL.Dtos.ProductCategory;
using BLL.Services.Interfaces;
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
        /// Create a Product Category
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateProductCategory([FromBody] ProductCategoryRequest productCategoryRequest)
        {
            _logger.Information($"POST api/productCategory/create START Request: {JsonSerializer.Serialize(productCategoryRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //create product Category
            BaseResponse<ProductCategoryResponse> response = await _productCategoryService.CreateProCategory(productCategoryRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/productCategory/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product Category By Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategoryById(string id)
        {
            _logger.Information($"GET api/productCategory/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get productCategory
            BaseResponse<ProductCategoryResponse> response = await _productCategoryService.GetProCategoryById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/productCategory/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product Category
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory(string id,
            [FromBody] ProductCategoryRequest productCategoryRequest)
        {
            _logger.Information($"PUT api/productCategory/{id} START Request: {JsonSerializer.Serialize(productCategoryRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update productCategory
            BaseResponse<ProductCategoryResponse> response = await _productCategoryService.UpdateProCategory(id, productCategoryRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/productCategory/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete ProductCategory by Id
        /// </summary>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteProductCategory(string id)
        {
            _logger.Information($"PUT api/productCategory/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete productCategory
            BaseResponse<ProductCategoryResponse> response = await _productCategoryService.DeleteProCategory(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/productCategory/delete/{id} END duration: " +
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
            BaseResponse<List<ProductCategoryResponse>> response =
                await _productCategoryService.GetProductCategoriesByStatus(status);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/productCategory/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
