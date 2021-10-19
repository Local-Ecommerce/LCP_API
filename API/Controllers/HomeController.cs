using BLL.Constants;
using BLL.Dtos;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }


        /*
         * [13/08/2021 - HanNQ] GET index
         */
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            _logger.Information($"Get api/home START Request:");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            BaseResponse<string> response = new BaseResponse<string>
            {
                ResultCode = ResultCode.SUCCESS_CODE,
                ResultMessage = ResultCode.SUCCESS_MESSAGE
            };

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("GET api/home END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
