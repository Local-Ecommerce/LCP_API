﻿using BLL.Dtos;
using BLL.Dtos.Resident;
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
    [Route("api/resident")]
    public class ResidentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IResidentService _residentService;

        public ResidentController(ILogger logger,
            IResidentService residentService)
        {
            _logger = logger;
            _residentService = residentService;
        }


        /// <summary>
        /// Create a Resident
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateResident([FromBody] ResidentRequest residentRequest)
        {
            _logger.Information($"POST api/resident/create START Request: " +
                $"{JsonSerializer.Serialize(residentRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //create Resident
            BaseResponse<ResidentResponse> response = await _residentService.CreateResident(residentRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/resident/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Resident By Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResidentById(string id)
        {
            _logger.Information($"GET api/resident/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get Resident
            BaseResponse<ResidentResponse> response = await _residentService.GetResidentById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/resident/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Resident
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResident(string id,
                                                      [FromBody] ResidentUpdateRequest residentUpdateRequest)
        {
            _logger.Information($"PUT api/resident/{id} START Request: " +
                $"{JsonSerializer.Serialize(residentUpdateRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //update Resident
            BaseResponse<ResidentResponse> response = await _residentService.UpdateResidentById(id, residentUpdateRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/resident/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Resident
        /// </summary>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteResident(string id)
        {
            _logger.Information($"PUT api/resident/delete/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete Resident
            BaseResponse<ResidentResponse> response = await _residentService.DeleteResident(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/resident/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Resident By Apartment Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetResidentByApartmentId(string apartmentId)
        {
            _logger.Information($"GET api/resident/apartment/{apartmentId} START");

            Stopwatch watch = new();
            watch.Start();

            //get Resident
            BaseResponse<List<ResidentResponse>> response =
                await _residentService.GetResidentByApartmentId(apartmentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/resident/apartment/{apartmentId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Resident By Account Id
        /// </summary>
        [AllowAnonymous]
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetResidentByAccountId(string accountId)
        {
            _logger.Information($"GET api/resident/account/{accountId} START");

            Stopwatch watch = new();
            watch.Start();

            //get Resident
            BaseResponse<List<ResidentResponse>> response =
                await _residentService.GetResidentByAccountId(accountId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/resident/account/{accountId} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
        
        
        /// <summary>
        /// Get All Residents
        /// </summary>
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllResidents()
        {
            _logger.Information($"GET api/resident/all START");

            Stopwatch watch = new();
            watch.Start();

            //get Resident
            BaseResponse<List<ResidentResponse>> response =
                await _residentService.GetAllResidents();

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/resident/all END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}