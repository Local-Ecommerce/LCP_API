using DAL.Constants;
using BLL.Dtos;
using BLL.Dtos.Menu;
using BLL.Dtos.ProductInMenu;
using BLL.Services.Interfaces;
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
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMenuService _menuService;

        public MenuController(ILogger logger, IMenuService menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        /// <summary>
        /// Create menu (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] MenuRequest menuRequest)
        {
            _logger.Information($"POST api/menu START Request: " +
                $"{JsonSerializer.Serialize(menuRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create Menu
            MenuResponse response = await _menuService.CreateMenu(menuRequest);

            string json = JsonSerializer.Serialize(ApiResponse<MenuResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/menu END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get menu by id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuById(string id)
        {
            _logger.Information($"GET api/menu/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Menu
            ExtendMenuResponse response = await _menuService.GetMenuById(id);

            string json = JsonSerializer.Serialize(ApiResponse<MenuResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/menu/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update menu (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuById(string id, [FromBody] MenuUpdateRequest menuUpdateRequest)
        {
            _logger.Information($"PUT api/menu/{id} START Request: " +
                $"{JsonSerializer.Serialize(menuUpdateRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update Menu
            MenuResponse response = await _menuService.UpdateMenuById(id, menuUpdateRequest);

            string json = JsonSerializer.Serialize(ApiResponse<MenuResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/menu/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Delete menu (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuById(string id)
        {
            _logger.Information($"DELETE api/menu/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete Menu
            MenuResponse response = await _menuService.DeleteMenuById(id);

            string json = JsonSerializer.Serialize(ApiResponse<MenuResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/menu/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Add Products To Menu (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost("{menuId}/products")]
        public async Task<IActionResult> AddProductsToMenu(string menuId,
            [FromBody] List<ProductInMenuRequest> productInMenuRequests)
        {
            _logger.Information($"POST api/menu/{menuId}/products START");

            Stopwatch watch = new();
            watch.Start();

            //Add Products To Menu
            List<ExtendProductInMenuResponse> response = await _menuService.AddProductsToMenu(menuId, productInMenuRequests);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"POST api/menu/merchant/{menuId}/products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Products In Menu By Menu Id (Merchant, Market Manager, Admin)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("{menuId}/products")]
        public async Task<IActionResult> GetProductsInMenuByMenuId(string menuId)
        {
            _logger.Information($"GET api/menu/{menuId}/products START");

            Stopwatch watch = new();
            watch.Start();

            //Get Product in Menu by Menu Id
            List<ExtendProductInMenuResponse> response = await _menuService.GetProductsInMenuByMenuId(menuId);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/menu/{menuId}/products END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product In Menu By Id (Merchant, Market Manager, Admin)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [Authorize(Roles = ResidentType.MERCHANT)]
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("products/{productInMenuId}")]
        public async Task<IActionResult> GetProductInMenuById(string productInMenuId)
        {
            _logger.Information($"GET api/menu/products/{productInMenuId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Product in Menu by Menu Id
            ExtendProductInMenuResponse response = await _menuService.GetProductInMenuById(productInMenuId);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductInMenuResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/menu/products/{productInMenuId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product In Menu By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut("products/{productInMenuId}")]
        public async Task<IActionResult> UpdateProductInMenuById(string productInMenuId,
            [FromBody] ProductInMenuUpdateRequest productInMenuUpdateRequest)
        {
            _logger.Information($"PUT api/menu/products/{productInMenuId} START");

            Stopwatch watch = new();
            watch.Start();

            //Update Product In Menu By Id
            ExtendProductInMenuResponse response = await _menuService
                .UpdateProductInMenuById(productInMenuId, productInMenuUpdateRequest);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendProductInMenuResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/menu/products/{productInMenuId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product In Menu By Id (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete("products/{productInMenuId}")]
        public async Task<IActionResult> DeleteProductInMenuById(string productInMenuId)
        {
            _logger.Information($"DELETE api/menu/products/{productInMenuId} START");

            Stopwatch watch = new();
            watch.Start();

            //Update Product In Menu By Id
            string response = await _menuService.DeleteProductInMenuById(productInMenuId);

            string json = JsonSerializer.Serialize(ApiResponse<string>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/menu/products/{productInMenuId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Menu By Status (Admin, Maket Manager)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetMenuByStatus(int status)
        {
            _logger.Information($"GET api/menu/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get Menu
            List<ExtendMenuResponse> response =
                await _menuService.GetMenusByStatus(status);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/menu/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get All Menus (Admin, Maket Manager)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMenus()
        {
            _logger.Information($"GET api/menu/all START");

            Stopwatch watch = new();
            watch.Start();

            //get Menu
            List<ExtendMenuResponse> response =
                await _menuService.GetAllMenus();

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/menu/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
