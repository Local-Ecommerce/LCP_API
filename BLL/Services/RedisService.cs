using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using BLL.Services.Interfaces;

namespace BLL.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger _logger;

        //contructor
        public RedisService(IDistributedCache distributedCache, ILogger logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        /*
         * [12/08/2021 - HanNQ] get List from Redis
         */
        public List<T> GetList<T>(string key)
        {
            string cache = _distributedCache.GetString(key);

            if (cache is null)
            {
                _logger.Information($"[RedisService.GetList()] No data for key '{key}'.");

                return default;
            }

            _logger.Information($"[RedisService.GetList()] Data for key '{key}': {cache}");

            return JsonSerializer.Deserialize<List<T>>(cache);
        }

        /*
         * [12/08/2021 - HanNQ] get string from Redis
         */
        public string GetString(string key)
        {
            string cache = _distributedCache.GetString(key);

            if (cache is not null)
            {
                _logger.Information($"[RedisService.GetString()] Data for key '{key}': {cache}");
                return cache;
            }

            _logger.Information($"[RedisService.GetString()] No data for key '{key}'.");

            return default;
        }

        /*
         * [12/08/2021 - HanNQ] store string to Redis
         */
        public void SetString(string key, string value)
        {
            _logger.Information($"[RedisService.SetString()] Set Data key '{key}': {value}");

            _distributedCache.SetString(key, value);
        }

        /*
         * [12/08/2021 - HanNQ] store List to Redis
         */
        public void StoreList<T>(string key, T value)
        {
            //convert data to json string
            string cache = JsonSerializer.Serialize(value);

            _logger.Information($"[RedisService.StoreList()] Set Data key '{key}': {value}");

            _distributedCache.SetString(key, cache);
        }
    }
}
