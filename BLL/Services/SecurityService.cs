﻿using BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ILogger _logger;
        private readonly IUtilService _utilService;

        public SecurityService(ILogger logger,
        IUtilService utilService)
        {
            _logger = logger;
            _utilService = utilService;
        }


        /// <summary>
        /// Get Signature
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreField"></param>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetSignature<T>(T obj, List<string> ignoreField, string accessKey, string secretKey)
        {
            string result = "accessKey=" + accessKey + "&";
            try
            {
                PropertyInfo[] props = obj.GetType().GetProperties();
                Array.Sort(props, (p1, p2) =>
                {
                    return p1.Name.CompareTo(p2.Name);
                });

                for (int i = 0; i < props.Length; i++)
                {
                    if (ignoreField.Contains(props[i].Name))
                    {
                        continue;
                    }
                    result += _utilService.LowerCaseFirstLetter(props[i].Name) + "=" + props[i].GetValue(obj, null);
                    if (i != props.Length - 1) result += "&";
                }

                if (result.EndsWith("&"))
                {
                    result = result.Remove(result.Length - 1, 1);
                }

                _logger.Information($"[SecurityService.GetSignature] Value: {result}");

                result = SignHmacSHA256(result, secretKey);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"[SecurityService.GetSignature()] Catch exception: {ex.Message}");
                return "";
            }
        }


        /// <summary>
        /// Sign Hmac SHA256
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string SignHmacSHA256(string rawData, string secretKey)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(rawData);
            if (string.IsNullOrEmpty(secretKey))
            {
                _logger.Error("[SignHmacSHA256] Missing secret key!");
                throw new Exception("Missing secret key!");
            }
            byte[] byteKey = Encoding.UTF8.GetBytes(secretKey);
            try
            {
                using HMACSHA256 hmacSHA256 = new(byteKey);
                byte[] hashResult = hmacSHA256.ComputeHash(byteData);
                string hexData = BitConverter.ToString(hashResult);
                hexData = hexData.Replace("-", "").ToLower();
                return hexData;
            }
            catch (Exception ex)
            {
                _logger.Error($"[SecurityService.SignHmacSHA256()] Error: {ex.Message}");
                return "";
            }
        }
    }
}
