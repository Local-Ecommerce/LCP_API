using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using BLL.Dtos;
using BLL.Dtos.MoMo.IPN;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/ipn")]
    public class IPNController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMoMoService _momoService;

        public IPNController(ILogger logger, IMoMoService moMoService)
        {
            _logger = logger;
            _momoService = moMoService;
        }

        /// <summary>
        /// Receive response from MoMo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ReceiveIPN([FromBody] MoMoIPNRequest momoIPNRequest)
        {

            _logger.Information($"POST api/ipn START Request: " +
                $"{JsonSerializer.Serialize(momoIPNRequest)}");

            Stopwatch watch = new();
            watch.Start();

            await _momoService.ProcessIPN(momoIPNRequest);

            string json = JsonSerializer.Serialize(ApiResponse<object>.Success());

            watch.Stop();

            _logger.Information("POST api/ipn END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}