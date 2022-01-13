using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IValidateDataService
    {
        /// <summary>
        /// Validate Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsValidName(string name);


        /// <summary>
        /// Validate Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        bool IsLaterThanPresent(DateTime? date);


        /// <summary>
        /// Validate Phone Number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        bool IsValidPhoneNumber(string phoneNumber);


        /// <summary>
        /// Validate Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool IsValidEmail(string email);
        
        
        /// <summary>
        /// Validate Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        bool IsValidPassword(string password);
    }
}
