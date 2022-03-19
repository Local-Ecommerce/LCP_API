using BLL.Services.Interfaces;
using System;

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
    }
}
