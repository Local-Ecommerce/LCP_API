using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.Resident
{
    [Serializable]
    public class ResidentUpdateRequest
    {
        [RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s\W|_]+$")]
        public string ResidentName { get; set; }
        [RegularExpression(@"^((09(\d){8})|(086(\d){7})|(088(\d){7})|(089(\d){7})|(01(\d){9}))$")]
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string DeliveryAddress { get; set; }
        public string ProfileImage { get; set; }
    }
}
