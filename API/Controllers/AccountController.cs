using DAL.Constants;
using BLL.Dtos;
using BLL.Dtos.Account;
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
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }


        /// <summary>
        /// Register
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterRequest accountRegisterRequest)
        {
            _logger.Information($"POST api/acccount/register START Request: " +
                $"{JsonSerializer.Serialize(accountRegisterRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //register account
            BaseResponse<ExtendAccountResponse> response = await _accountService.Register(accountRegisterRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/account/register END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Login
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginRequest accountLoginRequest)
        {
            _logger.Information($"GET api/acccount/login START Request: {JsonSerializer.Serialize(accountLoginRequest)}");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //Login
            BaseResponse<ExtendAccountResponse> response = await _accountService.Login(accountLoginRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("GET api/account/login END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get Account By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            _logger.Information($"GET api/account/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get account
            BaseResponse<ExtendAccountResponse> response = await _accountService.GetAccountById(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"GET api/account/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Account
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            _logger.Information($"PUT api/acccount/{id} START Request: ");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //update account
            BaseResponse<ExtendAccountResponse> response = await _accountService.UpdateAccount(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/account/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Account
        /// </summary>
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            _logger.Information($"PUT api/account/delete/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //delete account
            BaseResponse<ExtendAccountResponse> response = await _accountService.DeleteAccount(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/account/delete/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Change Resident Type By Account Id
        /// </summary>
        [HttpPut("{id}/residentType/{type}")]
        public async Task<IActionResult> ChangeRoleByAccountId(string id, string type)
        {
            _logger.Information($"PUT api/account/{id}/residentType/{type} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //change Role By Account
            BaseResponse<ExtendAccountResponse> response = await _accountService.ChangeResidentTypeByAccountId(id, type);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/account/{id}/residentType/{type} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
