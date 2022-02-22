using BLL.Dtos;
using BLL.Dtos.SystemCategory;
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
    [Route("api/categories")]
    public class SystemCategoryController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISystemCategoryService _systemCategoryService;

        public SystemCategoryController(ILogger logger,
            ISystemCategoryService systemCategoryService)
        {
            _logger = logger;
            _systemCategoryService = systemCategoryService;
        }

        /// <summary>
        /// Create a System Category (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreateSystemCategory([FromBody] SystemCategoryRequest systemCategoryRequest)
        {
            _logger.Information($"POST api/categories START Request: {JsonSerializer.Serialize(systemCategoryRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create system Category
            SystemCategoryResponse response = await _systemCategoryService.CreateSystemCategory(systemCategoryRequest);

            string json = JsonSerializer.Serialize(ApiResponse<SystemCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/categories END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get System Category (Authentication required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSystemCategory(
            [FromQuery] string id,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort)
        {
            _logger.Information($"GET api/categories?id={id}&limit={limit}&page={page}&sort={sort} START");

            Stopwatch watch = new();
            watch.Start();

            //get systemCategory
            object response = await _systemCategoryService.GetSystemCategory(id, limit, page, sort);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/categories?id={id}&limit={limit}&page={page}&sort={sort} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update SystemCategory By Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut]
        public async Task<IActionResult> UpdateSystemCategory([FromQuery] string id,
            [FromBody] SystemCategoryUpdateRequest systemCategoryRequest)
        {
            _logger.Information($"PUT api/categories?id={id} START Request: {JsonSerializer.Serialize(systemCategoryRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update systemCategory
            SystemCategoryResponse response = await _systemCategoryService.UpdateSystemCategory(id, systemCategoryRequest);

            string json = JsonSerializer.Serialize(ApiResponse<SystemCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/categories?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete SystemCategory by Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeleteSystemCategory([FromQuery] string id)
        {
            _logger.Information($"DELETE api/categories?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete systemCategory
            SystemCategoryResponse response = await _systemCategoryService.DeleteSystemCategory(id);

            string json = JsonSerializer.Serialize(ApiResponse<SystemCategoryResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/categories?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
