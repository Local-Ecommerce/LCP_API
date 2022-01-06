using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services.Interfaces;


namespace BLL.Services
{
    public class UtilService : IUtilService
    {
        /// <summary>
        /// Create 16 Alphanumeric for Id
        /// </summary>
        /// <returns></returns>
        public string Create16Alphanumeric()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }


        /// <summary>
        /// Check list is null or emply
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool IsNullOrEmpty<T>(IEnumerable<T> list)
        {
            return !(list?.Any()).GetValueOrDefault();
        }
    }
}
