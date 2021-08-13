using System.Collections.Generic;

namespace BLL.Services
{
    public interface IRedisService
    {
        /*
         * [12/08/2021 - HanNQ] store string to Redis
         */
        void SetString(string key, string value);

        /*
         * [12/08/2021 - HanNQ] get string from Redis
         */
        string GetString(string key);

        /*
         * [12/08/2021 - HanNQ] store List to Redis
         */
        void StoreList<T>(string key, T value);

        /*
         * [12/08/2021 - HanNQ] get List from Redis
         */
        List<T> GetList<T>(string key);

    }
}
