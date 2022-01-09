using BLL.Services.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace BLL.Services
{
    public class ValidateDataService : IValidateDataService
    {

        public static string PHONE_REGEX = @"^((09(\d){8})|(086(\d){7})|(088(\d){7})|(089(\d){7})|(01(\d){9}))$";
        public static string NAME_REGEX = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s\W|_]+$";


        /// <summary>
        /// Is Later Than Present
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsLaterThanPresent(DateTime? date)
        {
            if (date.HasValue)
            {
                if(date.Value.Date >= DateTime.Now.Date)
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Is Valid Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                try
                {
                    MailAddress mail = new(email);
                    return true;
                }
                catch (FormatException) 
                { 
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Is Valid Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsValidName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                    return Regex.IsMatch(name, NAME_REGEX);

            }
            return false;
        }


        /// <summary>
        /// Is Valid Phone Number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (!String.IsNullOrEmpty(phoneNumber))
            {
                return Regex.IsMatch(phoneNumber, PHONE_REGEX);
            }
            return false;
        }
    }
}
