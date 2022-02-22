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
    [Route("api/category-products")]
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
            _logger.Information($"POST api/category-products START Request: {JsonSerializer.Serialize(productCategoryRequest)}");

            Stopwatch watch = new ();
            watch.Start();

            //create product Category
            ProductCategoryResponse response = await _productCategoryService.CreateProCategory(productCategoryRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/category-products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product Category (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpGet]
        public async Task<IActionResult> GetProductCategory(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort)
        {
            _logger.Information($"GET api/category-products?id ={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort} START");

            Stopwatch watch = new ();
            watch.Start();

            //get productCategory
            object response = await _productCategoryService.GetProCategory(id, status, limit, page, sort);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/category-products?id ={id}&status=" + string.Join("status=", status) +
                $"&limit={limit}&page={page}&sort={sort} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product Category (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateProductCategory([FromQuery] string id,
            [FromBody] ProductCategoryRequest productCategoryRequest)
        {
            _logger.Information($"PUT api/category-products?id={id} START Request: {JsonSerializer.Serialize(productCategoryRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update productCategory
            ProductCategoryResponse response = await _productCategoryService.UpdateProCategory(id, productCategoryRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/category-products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete ProductCategory by Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductCategory([FromQuery] string id)
        {
            _logger.Information($"DELETE api/category-products?id={id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete productCategory
            ProductCategoryResponse response = await _productCategoryService.DeleteProCategory(id);

            string json = JsonSerializer.Serialize(ApiResponse<ProductCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/category-products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
