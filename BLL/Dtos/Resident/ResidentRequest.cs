using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.Resident
{
    [Serializable]
    public class ResidentRequest
    {
        [RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s\W|_]+$")]
        public string ResidentName { get; set; }

        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b")]
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string DeliveryAddress { get; set; }
        public string ApartmentId { get; set; }
    }
}
