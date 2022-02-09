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
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterRequest accountRegisterRequest)
        {
            _logger.Information($"POST api/acccount/signup START Request: " +
                $"{JsonSerializer.Serialize(accountRegisterRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //register account
            BaseResponse<AccountResponse> response = await _accountService.Register(accountRegisterRequest);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information("POST api/account/signup END duration: " +
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

            Stopwatch watch = new();
            watch.Start();

            //Login
            BaseResponse<AccountResponse> response = await _accountService.Login(accountLoginRequest);

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

            Stopwatch watch = new();
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            _logger.Information($"DELETE api/account/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete account
            BaseResponse<AccountResponse> response = await _accountService.DeleteAccount(id);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"DELETE api/account/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Change Resident Type By Account Id
        /// </summary>
        [HttpPut("{id}/type/{type}")]
        public async Task<IActionResult> ChangeRoleByAccountId(string id, string type)
        {
            _logger.Information($"PUT api/account/{id}/type/{type} START");

            Stopwatch watch = new();
            watch.Start();

            //change Role By Account
            BaseResponse<ExtendAccountResponse> response = await _accountService.ChangeResidentTypeByAccountId(id, type);

            string json = JsonSerializer.Serialize(response);

            watch.Stop();

            _logger.Information($"PUT api/account/{id}/type/{type} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
