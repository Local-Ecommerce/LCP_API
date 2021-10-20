using BLL.Dtos;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/product")]
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
        /// Create a product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public IActionResult CreateProduct([FromForm] ProductRequest productRequest,
            [FromForm] List<IFormFile> image)
        {
            _logger.Information($"POST api/product/create START Request: " +
                $"{JsonSerializer.Serialize(productRequest)}, {JsonSerializer.Serialize(image)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //create product
            BaseResponse<ProductResponse> response = _productService.CreateProduct(productRequest, image);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/product/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetProductById(string id)
        {
            _logger.Information($"GET api/product/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get product
            BaseResponse<ProductResponse> response = _productService.GetProductById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/product/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productRequest"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(string id, 
            [FromForm] ProductRequest productRequest,
            [FromForm] List<IFormFile> image)
        {
            _logger.Information($"PUT api/product/{id} START Request: " +
                $"{JsonSerializer.Serialize(productRequest)}, {JsonSerializer.Serialize(image)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update product
            BaseResponse<ProductResponse> response = _productService.UpdateProduct(id, productRequest, image);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/product/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public IActionResult DeleteProduct(string id)
        {
            _logger.Information($"PUT api/product/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get product
            BaseResponse<ProductResponse> response = _productService.DeleteProduct(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/product/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
