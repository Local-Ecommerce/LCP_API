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
        /// Get Signature
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreField"></param>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetSignature<T>(T obj, List<string> ignoreField, string accessKey, string secretKey);
    }
}
