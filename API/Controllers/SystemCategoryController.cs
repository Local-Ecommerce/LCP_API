using BLL.Dtos;
using BLL.Dtos.SystemCategory;
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
    [Route("api/systemCategory")]
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
        /// Create a System Category
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateSystemCategory([FromBody] SystemCategoryRequest systemCategoryRequest)
        {
            _logger.Information($"POST api/systemCategory/create START Request: {JsonSerializer.Serialize(systemCategoryRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //create system Category
            BaseResponse<SystemCategoryResponse> response = await _systemCategoryService.CreateSystemCategory(systemCategoryRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/systemCategory/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get System Category By Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSystemCategoryById(string id)
        {
            _logger.Information($"GET api/systemCategory/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get systemCategory
            BaseResponse<SystemCategoryResponse> response = await _systemCategoryService.GetSystemCategoryById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/systemCategory/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get System Category And One Level Down Inverse Belong To By Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("oneLevelDown/{id}")]
        public async Task<IActionResult> GetSystemCategoryAndOneLevelDownInverseBelongToById(string id)
        {
            _logger.Information($"GET api/systemCategory/oneLevelDown/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get systemCategory
            BaseResponse<SystemCategoryResponse> response = await _systemCategoryService.GetSystemCategoryAndOneLevelDownInverseBelongToById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/systemCategory/oneLevelDown/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get All System Category
        /// </summary>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSystemCategory()
        {
            _logger.Information($"GET api/systemCategory/all START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get systemCategory
            BaseResponse<List<SystemCategoryResponse>> response = await _systemCategoryService.GetAllSystemCategory();

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/systemCategory/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Update SystemCategory By Id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSystemCategory(string id,
            [FromBody] SystemCategoryRequest systemCategoryRequest)
        {
            _logger.Information($"PUT api/systemCategory/{id} START Request: {JsonSerializer.Serialize(systemCategoryRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update systemCategory
            BaseResponse<SystemCategoryResponse> response = await _systemCategoryService.UpdateSystemCategory(id, systemCategoryRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/systemCategory/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete SystemCategory by Id
        /// </summary>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteSystemCategory(string id)
        {
            _logger.Information($"PUT api/systemCategory/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete systemCategory
            BaseResponse<SystemCategoryResponse> response = await _systemCategoryService.DeleteSystemCategory(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/systemCategory/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get System Categories By Status
        /// </summary>
        [AllowAnonymous]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetSystemCategoriesByStatus(int status)
        {
            _logger.Information($"GET api/systemCategory/status/{status} START");

            Stopwatch watch = new();
            watch.Start();

            //get SystemCategory
            BaseResponse<List<SystemCategoryResponse>> response =
                await _systemCategoryService.GetSystemCategoriesByStatus(status);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/systemCategory/status/{status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get All Level One And Two System Category
        /// </summary>
        [AllowAnonymous]
        [HttpGet("autocomplete")]
        public async Task<IActionResult> GetAllLevelOneAndTwoSystemCategory()
        {
            _logger.Information($"GET api/systemCategory/autocomplete START");

            Stopwatch watch = new();
            watch.Start();

            //get SystemCategory
            BaseResponse<List<SystemCategoryForAutoCompleteResponse>> response =
                await _systemCategoryService.GetSystemCategoriesForAutoComplete();

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/systemCategory/autocomplete END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
