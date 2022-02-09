using DAL.Constants;
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

namespace API.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }


        /// <summary>
        /// Create Order
        /// </summary>
        [Authorize(Roles = ResidentType.CUSTOMER)]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderDetailRequest> orderDetailRequests)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            _logger.Information($"POST api/order START Request: " +
                $"{JsonSerializer.Serialize(orderDetailRequests)}" + $"Resident Id: {residentId}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //Create Order
            BaseResponse<List<ExtendOrderResponse>> response = await
            _orderService.CreateOrder(orderDetailRequests, residentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/order END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Order By Resident Id And Status
        /// </summary>
        [Authorize(Roles = ResidentType.CUSTOMER)]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetOrderByResidentIdAndStatus(int status)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            _logger.Information($"GET api/order/status/{status} START Request: " + $"Resident Id: {residentId}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //Get Order
            BaseResponse<List<ExtendOrderResponse>> response = await
            _orderService.GetOrderByResidentIdAndStatus(residentId, status);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/order/status/{status}  END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Order By Merchant Store Id
        /// </summary>
        // [Authorize(Roles = ResidentType.CUSTOMER)]
        [AllowAnonymous]
        [HttpGet("store/{merchantStoreId}")]
        public async Task<IActionResult> GetOrderByMerchantStoreId(string merchantStoreId)
        {
            // var identity = HttpContext.User.Identity as ClaimsIdentity;
            // IEnumerable<Claim> claim = identity.Claims;

            // string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            // string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            _logger.Information($"GET api/order/store/{merchantStoreId} START Request: ");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //Get Order
            BaseResponse<List<ExtendOrderResponse>> response = await
            _orderService.GetOrderByMerchantStoreId(merchantStoreId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/order/store/{merchantStoreId}  END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Order By Order Id And Resident Id
        /// </summary>
        [Authorize(Roles = ResidentType.CUSTOMER)]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrderByOrderIdAndResidentId(string orderId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            _logger.Information($"DELETE api/order/{orderId} START Request: " + $"Resident Id: {residentId}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //Delete Order
            BaseResponse<OrderResponse> response = await
            _orderService.DeleteOrderByOrderIdAndResidentId(orderId, residentId);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"DELETE api/order/{orderId}  END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
