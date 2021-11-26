using BLL.Dtos;
using BLL.Dtos.Merchant;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/merchant")]
    public class MerchantController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMerchantService _merchantService;

        public MerchantController(ILogger logger,
            IMerchantService merchantService)
        {
            _logger = logger;
            _merchantService = merchantService;
        }

        /// <summary>
        /// Create a Merchant
        /// </summary>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateMerchant([FromBody] MerchantRequest merchantRequest)
        {
            _logger.Information($"POST api/merchant/create START Request: " +
                $"{JsonSerializer.Serialize(merchantRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //create Merchant
            BaseResponse<MerchantResponse> response = await _merchantService.CreateMerchant(merchantRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/merchant/create END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }



        /// <summary>
        /// Get Merchant By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMerchantById(string id)
        {
            _logger.Information($"GET api/merchant/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get merchant
            BaseResponse<MerchantResponse> response = await _merchantService.GetMerchantById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/merchant/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMerchant(string id,
                                                      [FromBody] MerchantRequest merchantRequest)
        {
            _logger.Information($"PUT api/merchant/{id} START Request: " +
                $"{JsonSerializer.Serialize(merchantRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update merchant
            BaseResponse<MerchantResponse> response = await _merchantService.UpdateMerchantById(id, merchantRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/merchant/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteMerchant(string id)
        {
            _logger.Information($"PUT api/merchant/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete merchant
            BaseResponse<MerchantResponse> response = await _merchantService.DeleteMerchant(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/merchant/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
