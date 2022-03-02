using BLL.Services.Interfaces;
using System;

namespace BLL.Services
{
    public class ValidateDataService : IValidateDataService
    {

        public static string PHONE_REGEX = @"^((09(\d){8})|(086(\d){7})|(088(\d){7})|(089(\d){7})|(01(\d){9}))$";
        public static string NAME_REGEX = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s\W|_]+$";
        public static string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

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
