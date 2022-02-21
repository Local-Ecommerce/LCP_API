using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.ProductInMenu;
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
    [Route("api/menu-products")]
    public class ProductInMenuController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductInMenuService _productInMenuService;

        public ProductInMenuController(ILogger logger, IProductInMenuService productInMenuService)
        {
            _logger = logger;
            _productInMenuService = productInMenuService;
        }

        /// <summary>
        /// Add Products To Menu (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> AddProductsToMenu([FromQuery] string menuid,
            [FromBody] List<ProductInMenuRequest> productInMenuRequests)
        {
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
        /// Update Product In Menu By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateProductInMenuById([FromQuery]string id,
            [FromBody] ProductInMenuUpdateRequest productInMenuUpdateRequest)
        {
            _logger.Information($"PUT api/menu-products?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //Update Product In Menu By Id
            ExtendProductInMenuResponse response = await _productInMenuService
                .UpdateProductInMenuById(id, productInMenuUpdateRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductInMenuResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/menu-products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product In Menu By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductInMenuById([FromQuery]string id)
        {
            _logger.Information($"DELETE api/menu-products?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete Product In Menu By Id
            string response = await _productInMenuService.DeleteProductInMenuById(id);

            string json = JsonSerializer.Serialize(ApiResponse<string>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/menu-products?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
