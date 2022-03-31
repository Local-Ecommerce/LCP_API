using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.ProductInMenu;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/menu-products")]
    public class ProductInMenuController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductInMenuService _productInMenuService;
        private readonly ITokenService _tokenService;

        public ProductInMenuController(ILogger logger, IProductInMenuService productInMenuService,
        ITokenService tokenService)
        {
            _logger = logger;
            _productInMenuService = productInMenuService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Add Products To Menu (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> AddProductsToMenu([FromQuery] string menuid,
            [FromBody] List<ProductInMenuRequest> productInMenuRequests)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"POST api/menu-products?menuid={menuid} START");

            Stopwatch watch = new();
            watch.Start();

            //Add Products To Menu
            List<ExtendProductInMenuResponse> response = await _productInMenuService.AddProductsToMenu(menuid, productInMenuRequests);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"POST api/menu-products?menuid={menuid} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Products In Menu (Merchant, Market Manager, Admin)
        /// </summary>
        [AuthorizeRoles(RoleId.ADMIN, ResidentType.MERCHANT, ResidentType.MARKET_MANAGER)]
        [HttpGet]
        public async Task<IActionResult> GetProductsInMenu(
            [FromQuery] string id,
            [FromQuery] string menuid,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string include)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/menu-products?id={id}&menu={menuid}" +
                $"&limit={limit}&page={page}&sort={sort}&include={include} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Product in Menu
            object response = await _productInMenuService.GetProductsInMenu(id, menuid, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/menu-products?id={id}&menu={menuid}" +
                $"&limit={limit}&page={page}&sort={sort}&include={include} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Products In Menu (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateProductsInMenu(
            [FromBody] ListProductInMenuUpdateRequest productInMenuUpdateRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/menu-products START Request: "
                + string.Join("; ", productInMenuUpdateRequest.ProductInMenus));

            Stopwatch watch = new();
            watch.Start();

            //Update Products In Menu
            object response = await _productInMenuService.UpdateProductsInMenu(productInMenuUpdateRequest);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/menu-products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product In Menu By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductInMenuById([FromBody] List<string> ids)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"DELETE api/menu-products START Request: { ids}");

            Stopwatch watch = new();
            watch.Start();

            //Delete Product In Menu By Id
            await _productInMenuService.DeleteProductInMenu(ids);

            string json = JsonSerializer.Serialize(ApiResponse<string>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/menu-productsEND duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
