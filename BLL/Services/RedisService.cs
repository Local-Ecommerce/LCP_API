using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using BLL.Services.Interfaces;
using System;

namespace BLL.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger _logger;

        public RedisService(IDistributedCache distributedCache, ILogger logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public void DeleteFromList<T>(string listKey, Predicate<T> predicate)
        {
            List<T> list = GetList<T>(listKey);
            T t = list.Find(predicate);

            if (t != null)
            {
                list.Remove(t);
            }

            StoreList(listKey, list);
        }

        /// <summary>
        /// Get List of <T> from Redis by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>List of <T></returns>
        public List<T> GetList<T>(string key)
        {
            string cache = _distributedCache.GetString(key);

            if (string.IsNullOrEmpty(cache) || cache.Equals("[]"))
            {
                _logger.Information($"[RedisService.GetList()] No data for key '{key}'.");

                return new List<T>();
            }

            _logger.Information($"[RedisService.GetList()] Data for key '{key}': {cache}");

            return JsonSerializer.Deserialize<List<T>>(cache);
        }

        /// <summary>
        /// Get String by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>string or default</returns>
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

        /// <summary>
        /// Store string to Redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetString(string key, string value)
        {
            _logger.Information($"[RedisService.SetString()] Set Data key '{key}': {value}");

            _distributedCache.SetString(key, value);
        }

        /// <summary>
        /// Store list of <T> to Redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void StoreList<T>(string key, List<T> value)
        {
            //convert data to json string
            string cache = JsonSerializer.Serialize(value);

            _logger.Information($"[RedisService.StoreList()] Set Data key '{key}': {value}");

            _distributedCache.SetString(key, cache);
        }

        /// <summary>
        /// Store T to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listKey"></param>
        /// <param name="value"></param>
        /// <param name="predicate"></param>
        public void StoreToList<T>(string listKey, T value, Predicate<T> predicate)
        {
            List<T> list = GetList<T>(listKey);
            T t = list.Find(predicate);

            if (t != null)
            {
                list.Remove(t);
            }

            list.Add(value);
            StoreList(listKey, list);
        }
    }
}
