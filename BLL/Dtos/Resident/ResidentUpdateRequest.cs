using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.Resident
{
    public class ResidentUpdateRequest
    {
        public string ResidentName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string DeliveryAddress { get; set; }
        public string ApproveBy { get; set; }
        public string AccountId { get; set; }
        public string ApartmentId { get; set; }
    }
}
