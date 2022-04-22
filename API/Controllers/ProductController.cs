using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.Product;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
        private readonly ITokenService _tokenService;

        public ProductController(ILogger logger,
            IProductService productService, ITokenService tokenService)
        {
            _logger = logger;
            _productService = productService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Create Product (base include related product) (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateBaseProduct([FromBody] BaseProductRequest productRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"POST api/products START Request: {JsonSerializer.Serialize(productRequest)}");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //create product
            BaseProductResponse response = await _productService.CreateProduct(residentId, productRequest);

            string json = JsonSerializer.Serialize(ApiResponse<BaseProductResponse>.Success(response));

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
        public async Task<IActionResult> AddRelatedProduct(string id, [FromBody] ListProductRequest relatedProductRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"POST api/products/related/{id} START Request: {JsonSerializer.Serialize(relatedProductRequest)}");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //add related product
            await _productService.AddRelatedProduct(id, residentId, relatedProductRequest.Products);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success());

            watch.Stop();

            _logger.Information($"POST api/products/related/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product (Authorization required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProduct(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] string apartmentid,
            [FromQuery] string categoryid,
            [FromQuery] string search,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string[] include)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/products" +
                $"?id={id}&status=" + string.Join("status=", status) + $"&apartmentid={apartmentid}&categoryid={categoryid}" +
                $"&search={search}&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) + " START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //get role from token
            string claimRole = claim.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().ToString();
            string role = claimRole.Substring(claimRole.LastIndexOf(':') + 2);

            //get base product
            PagingModel<BaseProductResponse> response;
            switch (role)
            {
                case "Customer":
                    response = await _productService.GetProductForCustomer(id, residentId, categoryid, search);
                    break;
                default:
                    response = await _productService.GetProduct(id, status, apartmentid,
                                                categoryid, search, limit, page, sort, include, residentId, role);
                    break;
            }

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/products" +
                $"?id={id}&status=" + string.Join("status=", status) + $"&apartmentid={apartmentid}&categoryid={categoryid}" +
                $"&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) + " END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest productRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/products START Request: {JsonSerializer.Serialize(productRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update product
            await _productService.UpdateProduct(productRequest);

            string json = JsonSerializer.Serialize(ApiResponse<BaseProductResponse>.Success());

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
        public async Task<IActionResult> DeleteProduct([FromBody] ProductIdsRequest productIdsRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"DELETE api/products START Request: id="
            + string.Join(" ,id=", productIdsRequest.ProductIds));

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //delete products
            await _productService.DeleteProduct(productIdsRequest.ProductIds, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<BaseProductResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Approve  Product With ID (Admin, Market Manager)
        /// </summary>
        [AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
        [HttpPut("approval")]
        public async Task<IActionResult> ApproveProduct([FromQuery] string id)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/products/approval?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //approve Product
            BaseProductResponse response = await _productService.VerifyProductById(id, true, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<BaseProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/products/approval?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Reject Product With ID (Admin, Market Manager)
        /// </summary>
        [AuthorizeRoles(RoleId.ADMIN, ResidentType.MARKET_MANAGER)]
        [HttpPut("rejection")]
        public async Task<IActionResult> RejectProduct([FromQuery] string id)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/products/rejection?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //reject Product
            BaseProductResponse response = await _productService.VerifyProductById(id, false, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<BaseProductResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/products/rejection?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
