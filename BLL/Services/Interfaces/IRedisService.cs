using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IRedisService
    {
        /// <summary>
        /// Store string to Redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetString(string key, string value);


        /// <summary>
        /// Get String by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>string or default</returns>
        string GetString(string key);


        /// <summary>
        /// Store list of <T> to Redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void StoreList<T>(string key, List<T> value);


        /// <summary>
        /// Get List of <T> from Redis by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>List of <T></returns>
        List<T> GetList<T>(string key);

    }
}
