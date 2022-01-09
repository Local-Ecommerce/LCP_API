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

    }
}
