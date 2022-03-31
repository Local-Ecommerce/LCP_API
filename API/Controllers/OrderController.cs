﻿using DAL.Constants;
using BLL.Dtos;
using BLL.Dtos.Order;
using BLL.Dtos.OrderDetail;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace API.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly ITokenService _tokenService;

        public OrderController(ILogger logger,
            IOrderService orderService, ITokenService tokenService)
        {
            _logger = logger;
            _orderService = orderService;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Create Order (Customer)
        /// </summary>
        [Authorize(Roles = ResidentType.CUSTOMER)]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderDetailRequest> orderDetailRequests)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            _logger.Information($"POST api/orders START Request: " +
                $"{JsonSerializer.Serialize(orderDetailRequests)}" + $"Resident Id: {residentId}");

            Stopwatch watch = new();
            watch.Start();

            //Create Order
            List<ExtendOrderResponse> response = await
            _orderService.CreateOrder(orderDetailRequests, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information("POST api/orders END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Order (Authentication required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrder(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] string merchantstoreid,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string include)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"GET api/orders?id={id}&status=" + string.Join("status=", status) +
                $"&merchantstoreid={merchantstoreid}&limit={limit}&page={page}&sort={sort}&include={include}"
                + " START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //get role from token
            string claimRole = claim.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().ToString();
            string role = claimRole.Substring(claimRole.LastIndexOf(':') + 2);

            //Get Order
            object response = await
            _orderService.GetOrders(id, residentId, role, merchantstoreid, status, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/orders?id={id}&status=" + string.Join("status=", status) +
                $"&merchantstoreid={merchantstoreid}&limit={limit}&page={page}&sort={sort}&include="
                + string.Join("include=", include) + " END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Order (Authentication required)
        /// </summary>

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] string id, [FromQuery] int status)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            _logger.Information($"PUT api/orders?id={id}&status={status} START Request: ");

            Stopwatch watch = new();
            watch.Start();

            //Update Order
            OrderResponse response = await
            _orderService.UpdateOrderStatus(id, status);

            string json = JsonSerializer.Serialize(ApiResponse<OrderResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/orders?id={id}&status={status}  END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Delete Order By Order Id (Customer)
        /// </summary>
        [Authorize(Roles = ResidentType.CUSTOMER)]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderByOrderIdAndResidentId([FromQuery] string orderId)
        {
            //check token expired
            _tokenService.CheckTokenExpired(Request.Headers[HeaderNames.Authorization]);

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            _logger.Information($"DELETE api/orders?orderid={orderId} START Request: " + $"Resident Id: {residentId}");

            Stopwatch watch = new();
            watch.Start();

            //Delete Order
            await _orderService.DeleteOrderByOrderId(orderId, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<OrderResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/orders?orderid={orderId}  END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
