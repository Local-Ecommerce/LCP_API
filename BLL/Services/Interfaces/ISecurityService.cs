using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface ISecurityService
    {
        /// <summary>
        /// Sign Hmac SHA256
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        string SignHmacSHA256(string rawData, string secretKey);


        /// <summary>
        /// Get Raw Data Signature
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="ignoreField"></param>
        /// <returns></returns>
        string GetRawDataSignature<T>(T obj, List<string> ignoreField);
    }
}
