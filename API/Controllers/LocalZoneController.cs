using BLL.Dtos;
using BLL.Dtos.LocalZone;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/localZone")]
    public class LocalZoneController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILocalZoneService _localZoneService;

        public LocalZoneController(ILogger logger,
            ILocalZoneService localZoneService)
        {
            _logger = logger;
            _localZoneService = localZoneService;
        }


        /// <summary>
        /// Create LocalZone
        /// </summary>
        /// <param name="localZoneRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateLocalZone([FromBody] LocalZoneRequest localZoneRequest)
        {
            _logger.Information($"POST api/localZone/create START Request: " +
                $"{JsonSerializer.Serialize(localZoneRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create LocalZone
            BaseResponse<LocalZoneResponse> response = await _localZoneService.CreateLocalZone(localZoneRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/LocalZone/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get LocalZone By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocalZoneById(string id)
        {
            _logger.Information($"GET api/localZone/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //get LocalZone
            BaseResponse<LocalZoneResponse> response = await _localZoneService.GetLocalZoneById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/localZone/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update LocalZone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="localZoneRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocalZoneById(string id,
                                              [FromBody] LocalZoneRequest localZoneRequest)
        {
            _logger.Information($"PUT api/LocalZone/{id} START Request: " +
                $"{JsonSerializer.Serialize(localZoneRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update LocalZone
            BaseResponse<LocalZoneResponse> response = await _localZoneService.UpdateLocalZoneById(id, localZoneRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/localZone/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete localZone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeletelocalZone(string id)
        {
            _logger.Information($"PUT api/localZone/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete LocalZone
            BaseResponse<LocalZoneResponse> response = await _localZoneService.DeleteLocalZone(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/localZone/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
