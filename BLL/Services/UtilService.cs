using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services.Interfaces;


namespace BLL.Services
{
    public class UtilService : IUtilService
    {
        /// <summary>
        /// Create Id with prefix
        /// </summary>
        /// <returns></returns>
        public string CreateId(string prefix)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return string.Concat(prefix, new string(stringChars));
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


        /// <summary>
        /// Change To International Phone Number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string ChangeToInternationalPhoneNumber(string phone)
        {
            return phone.StartsWith("0") ? "+84" + phone[1..] : phone;
        }


        /// <summary>
        /// Change To Vietnam Phone Number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string ChangeToVietnamPhoneNumber(string phone)
        {
            return phone.StartsWith("+84") ? "0" + phone[3..] : phone;
        }


        /// <summary>
        /// Last Image Number
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public int LastImageNumber(string imageName, string url)
        {
            int t1 = url.LastIndexOf(imageName) + imageName.Length;
            int t2 = url.LastIndexOf("?");
            string order = url[t1..t2];
            if (order.Contains("."))
                order = order[0..order.LastIndexOf(".")];

            return string.IsNullOrEmpty(order) ? 0 : int.Parse(order);
        }


        /// <summary>
        /// Upper Case First Letter
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string UpperCaseFirstLetter(string str)
        {
            if (!string.IsNullOrEmpty(str))
                return str[0].ToString().ToUpper() + str[1..];
            return str;
        }
    }
}
