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
        /// Create menu
        /// </summary>
/*        [Authorize(Roles = ResidentType.MERCHANT)]*/
        [HttpPost("create")]
        public async Task<IActionResult> CreateMenu([FromBody] MenuRequest menuRequest)
        {
            _logger.Information($"POST api/menu/create START Request: " +
                $"{JsonSerializer.Serialize(menuRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Create Menu
            BaseResponse<MenuResponse> response = await _menuService.CreateMenu(menuRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/menu/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get menu by id
        /// </summary>
/*        [Authorize(Roles = ResidentType.MERCHANT)]*/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuById(string id)
        {
            _logger.Information($"GET api/menu/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Menu
            BaseResponse<MenuResponse> response = await _menuService.GetMenuById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/menu/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update menu
        /// </summary>
/*        [Authorize(Roles = ResidentType.MERCHANT)]*/
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMenuById(string id, [FromBody] MenuUpdateRequest menuUpdateRequest)
        {
            _logger.Information($"PUT api/menu/update/{id} START Request: " +
                $"{JsonSerializer.Serialize(menuUpdateRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Update Menu
            BaseResponse<MenuResponse> response = await _menuService.UpdateMenuById(id, menuUpdateRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/menu/update/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Delete menu
        /// </summary>
        // [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteMenuById(string id)
        {
            _logger.Information($"PUT api/menu/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //Delete Menu
            BaseResponse<MenuResponse> response = await _menuService.DeleteMenuById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/menu/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Add Products To Menu
        /// </summary>
/*        [Authorize(Roles = ResidentType.MERCHANT)]*/
        [HttpPost("{menuId}/add")]
        public async Task<IActionResult> AddProductsToMenu(string menuId,
            [FromBody] List<ProductInMenuRequest> productInMenuRequests)
        {
            _logger.Information($"POST api/menu/{menuId}/add START");

            Stopwatch watch = new();
            watch.Start();

            //Add Products To Menu
            BaseResponse<List<ProductInMenuResponse>> response = await _menuService.AddProductsToMenu(menuId, productInMenuRequests);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"POST api/menu/merchant/{menuId}/add END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Products In Menu By Menu Id
        /// </summary>
        // [Authorize(Roles = ResidentType.MERCHANT)]
        // [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("{menuId}/product")]
        public async Task<IActionResult> GetProductsInMenuByMenuId(string menuId)
        {
            _logger.Information($"GET api/menu/{menuId}/product START");

            Stopwatch watch = new();
            watch.Start();

            //Get Product in Menu by Menu Id
            BaseResponse<List<ProductInMenuResponse>> response = await _menuService.GetProductsInMenuByMenuId(menuId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/menu/{menuId}/product END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Product In Menu By Id
        /// </summary>
        // [Authorize(Roles = ResidentType.MERCHANT)]
        // [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("product/{productInMenuId}")]
        public async Task<IActionResult> GetProductInMenuById(string productInMenuId)
        {
            _logger.Information($"GET api/menu/product/{productInMenuId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Product in Menu by Menu Id
            BaseResponse<ProductInMenuResponse> response = await _menuService.GetProductInMenuById(productInMenuId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/menu/product/{productInMenuId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Product In Menu By Id
        /// </summary>
/*        [Authorize(Roles = ResidentType.MERCHANT)]*/
        [HttpPut("product/{productInMenuId}")]
        public async Task<IActionResult> UpdateProductInMenuById(string productInMenuId,
            [FromBody] ProductInMenuUpdateRequest productInMenuUpdateRequest)
        {
            _logger.Information($"PUT api/menu/product/{productInMenuId} START");

            Stopwatch watch = new();
            watch.Start();

            //Update Product In Menu By Id
            BaseResponse<ProductInMenuResponse> response = await _menuService
                .UpdateProductInMenuById(productInMenuId, productInMenuUpdateRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/menu/product/{productInMenuId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Product In Menu By Id
        /// </summary>
/*        [Authorize(Roles = ResidentType.MERCHANT)]*/
        [HttpDelete("product/{productInMenuId}")]
        public async Task<IActionResult> DeleteProductInMenuById(string productInMenuId)
        {
            _logger.Information($"DELETE api/menu/product/{productInMenuId} START");

            Stopwatch watch = new();
            watch.Start();

            //Update Product In Menu By Id
            BaseResponse<string> response = await _menuService.DeleteProductInMenuById(productInMenuId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"DELETE api/menu/product/{productInMenuId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Menu By Status
        /// </summary>
        // [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetMenuByStatus(int status)
        {
            _logger.Information($"GET api/menu/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get Menu
            BaseResponse<List<MenuResponse>> response =
                await _menuService.GetMenusByStatus(status);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/menu/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get All Menus
        /// </summary>
        // [Authorize(Roles = RoleId.ADMIN)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMenus()
        {
            _logger.Information($"GET api/menu/all START");

            Stopwatch watch = new();
            watch.Start();

            //get Menu
            BaseResponse<List<MenuResponse>> response =
                await _menuService.GetAllMenus();

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/menu/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
