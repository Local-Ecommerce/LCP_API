using BLL.Dtos;
using BLL.Dtos.Account;
using BLL.Services.Interfaces;
using DAL.Constants;
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
        /// Login
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountRequest accountRequest)
        {
            _logger.Information($"GET api/acccount/login START Request: {JsonSerializer.Serialize(accountRequest)}");

            Stopwatch watch = new();
            watch.Start();

            //Login
            AccountResponse response = await _accountService.Login(accountRequest);

            string json = JsonSerializer.Serialize(ApiResponse<AccountResponse>.Success(response));

            watch.Stop();

            _logger.Information("GET api/account/login END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }

        /// <summary>
        /// Get Account By Id (Authentication required)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            _logger.Information($"GET api/account/{id} START");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            //get account
            ExtendAccountResponse response = await _accountService.GetAccountById(id);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendAccountResponse>.Success(response));

            watch.Stop();

            _logger.Information($"GET api/account/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Update Account (Authentication required)
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            _logger.Information($"PUT api/acccount/{id} START Request: ");

            Stopwatch watch = new();
            watch.Start();

            //update account
            ExtendAccountResponse response = await _accountService.UpdateAccount(id);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendAccountResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/account/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Delete Account (Authentication required)
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            _logger.Information($"DELETE api/account/{id} START");

            Stopwatch watch = new();
            watch.Start();

            //delete account
            AccountResponse response = await _accountService.DeleteAccount(id);

            string json = JsonSerializer.Serialize(ApiResponse<AccountResponse>.Success(response));

            watch.Stop();

            _logger.Information($"DELETE api/account/{id} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }


        /// <summary>
        /// Change Resident Type By Account Id (Admin)
        /// </summary>
        [Authorize(Roles = RoleId.ADMIN)]
        [HttpPut("{id}/type/{type}")]
        public async Task<IActionResult> ChangeRoleByAccountId(string id, string type)
        {
            _logger.Information($"PUT api/account/{id}/type/{type} START");

            Stopwatch watch = new();
            watch.Start();

            //change Role By Account
            ExtendAccountResponse response = await _accountService.ChangeResidentTypeByAccountId(id, type);

            string json = JsonSerializer.Serialize(ApiResponse<ExtendAccountResponse>.Success(response));

            watch.Stop();

            _logger.Information($"PUT api/account/{id}/type/{type} END duration: " +
                $"{watch.ElapsedMilliseconds} ms -----------Response: " + json);

            return Ok(json);
        }
    }
}
