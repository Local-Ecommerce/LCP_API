using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IUtilService
    {
        /// <summary>
        /// Create 16 Alphanumeric for Id
        /// </summary>
        /// <returns></returns>
        string Create16Alphanumeric();

        /// <summary>
        /// Check list is null or emply
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        bool IsNullOrEmpty<T>(IEnumerable<T> list);
    }
}
