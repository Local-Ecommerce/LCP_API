using System;
using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IUtilService
    {
        /// <summary>
        /// Create Id with prefix
        /// </summary>
        /// <returns></returns>
        string CreateId(string prefix);


        /// <summary>
        /// Check list is null or emply
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        bool IsNullOrEmpty<T>(IEnumerable<T> list);


        /// <summary>
        /// Change To International Phone Number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        string ChangeToInternationalPhoneNumber(string phone);


        /// <summary>
        /// Change To Vietnam Phone Number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        string ChangeToVietnamPhoneNumber(string phone);


        /// <summary>
        /// Last Image Number
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        int LastImageNumber(string imageName, string url);


        /// <summary>
        /// Upper Case First Letter
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string UpperCaseFirstLetter(string str);

        /// <summary>
        /// Current Time In Vietnam
        /// </summary>
        /// <returns></returns>
        DateTime CurrentTimeInVietnam();
    }
}
