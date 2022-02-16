using BLL.Dtos.JWT;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class CheckBlacklistTokenMiddleware : IMiddleware
    {
        private readonly IRedisService _redisService;
        private readonly ILogger _logger;
        private const string TOKEN_BLACKLIST_KEY = "Token Blacklist";

        public CheckBlacklistTokenMiddleware(IRedisService redisService,
            ILogger logger)
        {
            _redisService = redisService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                string JWTtoken = context.Request.Headers["Authorization"];
                string token = JWTtoken?[7..];

                if (!string.IsNullOrEmpty(token))
                {
                    //remove expired token from blacklist
                    _redisService.DeleteFromList<TokenInfo>(TOKEN_BLACKLIST_KEY,
                        new Predicate<TokenInfo>(ti => DateTime.Compare((DateTime)ti.ExpiredDate, DateTime.Now) <= 0));

                    if (_redisService.GetList<TokenInfo>(TOKEN_BLACKLIST_KEY).Find(ti => ti.Token.Equals(token)) != null)
                        throw new UnauthorizedAccessException();
                }
            }
            catch (Exception)
            {
                _logger.Error("[CheckBlacklistTokenMiddleware]: Token is in blacklist.");
                throw;
            }

            await next(context);
        }
    }
}
