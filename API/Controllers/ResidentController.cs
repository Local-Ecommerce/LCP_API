﻿using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.Resident;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/residents")]
    public class ResidentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IResidentService _residentService;
        private readonly ITokenService _tokenService;

        public ResidentController(ILogger logger,
            IResidentService residentService, ITokenService tokenService)
        {
            _logger = logger;
            _residentService = residentService;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Get Resident (Authentication required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetResident(
            [FromQuery] string id,
            [FromQuery] string apartmentid,
            [FromQuery] string accountid,
            [FromQuery] string phonenumber,
            [FromQuery] int?[] status,
            [FromQuery] string type,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/residents?id={id}&apartmentid={apartmentid}&phonenumber={phonenumber}&accountid={accountid}&status=" + string.Join("status=", status) + $"&limit={limit}&page={page}&sort={sort} START");

            Stopwatch watch = new();
            watch.Start();

            //get Resident
            object response = await _residentService.GetResident(id, apartmentid, phonenumber, accountid, status, type, limit, page, sort);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/residents?id={id}&apartmentid={apartmentid}&phonenumber={phonenumber}&accountid={accountid}&status=" +
                string.Join("status=", status) + $"&limit={limit}&page={page}&sort={sort} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Resident (Apartment Roles)
        /// </summary>
        [AuthorizeRoles(ResidentType.CUSTOMER, ResidentType.MERCHANT, ResidentType.MARKET_MANAGER)]
        [HttpPut]
        public async Task<IActionResult> UpdateResident([FromQuery] string id,
                                                      [FromBody] ResidentUpdateRequest residentUpdateRequest)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/residents?id={id} START Request: " +
                $"{JsonSerializer.Serialize(residentUpdateRequest)}");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get role from token
            string claimRole = claim.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().ToString();
            string role = claimRole.Substring(claimRole.LastIndexOf(':') + 2);

            //update Resident
            ResidentResponse response = await _residentService.UpdateResidentById(id, residentUpdateRequest, role);

            string json = JsonSerializer.Serialize(ApiResponse<ResidentResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/residents?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Resident Status (Market Manager)
        /// </summary>
        [AuthorizeRoles(ResidentType.MARKET_MANAGER, RoleId.ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResidentStatus(string id, [FromQuery] int status)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/residents/{id}?status={status} START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string managerId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //get role from token
            string claimRole = claim.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().ToString();
            string role = claimRole.Substring(claimRole.LastIndexOf(':') + 2);

            //Update Resident status
            ResidentResponse response = await _residentService.UpdateResidentStatus(id, status, managerId, role);

            string json = JsonSerializer.Serialize(ApiResponse<ResidentResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/residents/{id}?status={status} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Resident (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeleteResident([FromQuery] string id)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"DELETE api/residents?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete Resident
            await _residentService.DeleteResident(id);

            string json = JsonSerializer.Serialize(ApiResponse<ResidentResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/residents?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
