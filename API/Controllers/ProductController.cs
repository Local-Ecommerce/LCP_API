using BLL.Dtos;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //create product
            ExtendProductResponse response = await _productService.CreateProduct(residentId, productRequest);

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

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //add related product
            object response = await _productService.AddRelatedProduct(id, residentId, relatedProductRequest);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

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
            [FromQuery] string apartmentid,
            [FromQuery] string type,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string[] include)
        {
            _logger.Information($"GET api/products" +
                $"?id={id}&status=" + string.Join("status=", status) + $"&apartmentid={apartmentid}&type={type}" +
                $"&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) + " START");

            Stopwatch watch = new();
            watch.Start();

            //get base product
            object response = await _productService.GetProduct(id, status, apartmentid, type, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/products" +
                $"?id={id}&status=" + string.Join("status=", status) + $"&apartmentid={apartmentid}&type={type}" +
                $"&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) + " END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product (Merchant)
        /// </summary>
        // [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] List<UpdateProductRequest> productRequests)
        {
            _logger.Information($"PUT api/products START Request: {JsonSerializer.Serialize(productRequests)}");

            Stopwatch watch = new();
            watch.Start();

            //update product
            await _productService.UpdateProduct(productRequests);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductResponse>.Success());

            watch.Stop();

            _logger.Information($"PUT api/products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product by Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromBody] List<string> ids)
        {
            _logger.Information($"DELETE api/products START Request: id="
            + string.Join(" ,id=", ids));

            Stopwatch watch = new();
            watch.Start();

            //delete products
            await _productService.DeleteProduct(ids);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/products END duration: " +
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
            ExtendProductResponse response = await _productService.VerifyProductById(id, true);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductResponse>.Success(response));

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
            ExtendProductResponse response = await _productService.VerifyProductById(id, false);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/products/rejection?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
