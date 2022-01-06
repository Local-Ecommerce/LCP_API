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

    }
}
