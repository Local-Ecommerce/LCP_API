using BLL.Services.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace BLL.Services
{
    public class ValidateDataService : IValidateDataService
    {
        /// <summary>
        /// Is Later Than Present
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsLaterThanPresent(DateTime? date)
        {
            if (date.HasValue)
            {
                if (date.Value.Date >= DateTime.Now.Date)
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Is Valid Time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsValidTime(string time)
        {
            var regex = new Regex(@"^(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)$");

            return regex.IsMatch(time);
        }
    }
}
