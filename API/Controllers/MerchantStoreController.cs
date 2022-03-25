using API.Extensions;
using BLL.Dtos;
using BLL.Dtos.MerchantStore;
using BLL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/stores")]
    public class MerchantStoreController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMerchantStoreService _merchantStoreService;

        public MerchantStoreController(ILogger logger,
            IMerchantStoreService merchantStoreService)
        {
            _logger = logger;
            _merchantStoreService = merchantStoreService;
        }

        /// <summary>
        /// Create a Merchant Store (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPost]
        public async Task<IActionResult> CreateMerchantStore([FromBody] MerchantStoreRequest merchantStoreRequest)
        {
            _logger.Information($"POST api/stores START Request: " +
                $"{JsonSerializer.Serialize(merchantStoreRequest)}");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //create MerchantStore
            MerchantStoreResponse response = await _merchantStoreService.CreateMerchantStore(residentId, merchantStoreRequest);

            string json = JsonSerializer.Serialize(ApiResponse<MerchantStoreResponse>.Success(response));

            watch.Stop();

            _logger.Information("POST api/stores END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Merchant Store (Authentication required)
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMerchantStore(
            [FromQuery] string id,
            [FromQuery] int?[] status,
            [FromQuery] string apartmentid,
            [FromQuery] int? limit,
            [FromQuery] int? page,
            [FromQuery] string sort,
            [FromQuery] string[] include)
        {

            _logger.Information($"GET api/stores?id={id}&status=" + string.Join("status=", status) +
                $"&apartmentid={apartmentid}&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) +
                $" START");

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

            //Get MerchantStore
            object response = await _merchantStoreService.GetMerchantStores(id, apartmentid, residentId, role, status, limit, page, sort, include);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/stores?id={id}&status=" + string.Join("status=", status) +
                $"&apartmentid={apartmentid}&limit={limit}&page={page}&sort={sort}&include=" + string.Join("include=", include) +
                $"END duration: {watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Merchant Store (Merchant)
        /// </summary>
        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpPut]
        public async Task<IActionResult> UpdateMerchantStore([FromQuery] string id,
                                                      [FromBody] MerchantStoreRequest request)
        {
            _logger.Information($"PUT api/stores?id={id}  START Request: " +
                $"{JsonSerializer.Serialize(request)}");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //update MerchantStore
            await _merchantStoreService.UpdateMerchantStoreById(id, request, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendMerchantStoreResponse>.Success());

            watch.Stop();

            _logger.Information($"PUT api/stores?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Merchant Store (Merchant)
        /// </summary>

        [Authorize(Roles = ResidentType.MERCHANT)]
        [HttpDelete]
        public async Task<IActionResult> DeleteMerchantStore([FromQuery] string id)
        {
            _logger.Information($"DELETE api/stores?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //delete MerchantStore
            await _merchantStoreService.DeleteMerchantStore(id, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<MerchantStoreResponse>.Success());

            watch.Stop();

            _logger.Information($"DELETE api/stores?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Approve MerchantStore With ID (Admin, Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpPut("approval")]
        public async Task<IActionResult> ApproveMerchantStore([FromQuery] string id)
        {
            _logger.Information($"PUT api/stores/approval?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //approve MerchantStore
            ExtendMerchantStoreResponse response = await _merchantStoreService.VerifyMerchantStore(id, true, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendMerchantStoreResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/stores/approval?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Reject MerchantStore With ID (Admin, Market Manager)
        /// </summary>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpPut("rejection")]
        public async Task<IActionResult> RejectCreateMerchantStore([FromQuery] string id)
        {
            _logger.Information($"PUT api/stores/rejection?id={id} START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //reject MerchantStore
            ExtendMerchantStoreResponse response = await _merchantStoreService.VerifyMerchantStore(id, false, residentId);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendMerchantStoreResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/stores/rejection?id={id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Get Unverified Merchant Stores
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = ResidentType.MARKET_MANAGER)]
        [HttpGet("unverified-stores")]
        public async Task<IActionResult> GetUnverifiedMerchantStores()
        {
            _logger.Information("GET api/stores/unverified-stores START");

            Stopwatch watch = new();
            watch.Start();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;

            //get resident id from token
            string claimName = claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().ToString();
            string residentId = claimName.Substring(claimName.LastIndexOf(':') + 2);

            //get unverified MerchantStore
            List<ExtendMerchantStoreResponse> responses = await _merchantStoreService.GetUnverifiedMerchantStores(residentId);

            string json = JsonSerializer.Serialize(ApiResponse<List<ExtendMerchantStoreResponse>>.Success(responses));

            watch.Stop();

            _logger.Information("GET api/stores/unverified-stores END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

    }
}
