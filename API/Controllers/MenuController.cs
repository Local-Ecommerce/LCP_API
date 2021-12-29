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
        /// <param name="menuRequest"></param>
        /// <returns></returns>
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
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
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
        /// <param name="id"></param>
        /// <param name="menuUpdateRequest"></param>
        /// <returns></returns>
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
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Get Menus By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet("merchant/{merchantId}")]
        public async Task<IActionResult> GetMenusByMerchantId(string merchantId)
        {
            _logger.Information($"GET api/menu/merchant/{merchantId} START");

            Stopwatch watch = new();
            watch.Start();

            //Get Menu by Merchant Id
            BaseResponse<List<MenuResponse>> response = await _menuService.GetMenusByMerchantId(merchantId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/menu/merchant/{merchantId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Add Products To Menu
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="productInMenuRequests"></param>
        /// <returns></returns>
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
        /// <param name="menuId"></param>
        /// <returns></returns>
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
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
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
        /// <param name="productInMenuId"></param>
        /// <param name="productInMenuUpdateRequest"></param>
        /// <returns></returns>
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
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
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
    }
}
