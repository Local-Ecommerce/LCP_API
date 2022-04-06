using System;

namespace BLL.Services.Interfaces
{
    public interface IValidateDataService
    {
        /// <summary>
        /// Validate Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        bool IsLaterThanPresent(DateTime? date);


        /// <summary>
        /// Is Valid Time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        bool IsValidTime(string time);
    }
}
