﻿using BLL.Dtos;
using BLL.Dtos.SystemCategory;
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
    [Route("api/categories")]
    public class SystemCategoryController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISystemCategoryService _systemCategoryService;
        private readonly ITokenService _tokenService;

        public SystemCategoryController(ILogger logger,
            ISystemCategoryService systemCategoryService, ITokenService tokenService)
        {
            _logger = logger;
            _systemCategoryService = systemCategoryService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Create a System Category (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreateSystemCategory([FromBody] SystemCategoryRequest systemCategoryRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

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
            [FromQuery] string merchantid,
            [FromQuery] int?[] status,
            [FromQuery] string search,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string include)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/categories?id={id}&merchantid={merchantid}&status=" + string.Join("status=", status) +
            $"&limit={limit}&search={search}&page={page}&sort={sort}&include={include} START");

            Stopwatch watch = new();
            watch.Start();

            //get systemCategory
            object response = await _systemCategoryService
                .GetSystemCategories(id, merchantid, status, search, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/categories?id={id}&merchantid={merchantid}&status=" + string.Join("status=", status) +
            $"&search={search}&limit={limit}&page={page}&sort={sort}&include={include} END duration: " +
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
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

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
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"DELETE api/categories?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete systemCategory
            await _systemCategoryService.DeleteSystemCategory(id);

            string json = JsonSerializer.Serialize(ApiResponse<SystemCategoryResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/categories?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
