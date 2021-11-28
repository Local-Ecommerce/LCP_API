using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.Customer
{
    public class CustomerRequest
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string AccountId { get; set; }


        public enum CustomerStatus
        {
            ERROR = -1,
            SUCCESS = 0,
            CUSTOMER_NOT_FOUND = 8001,
            DELETED_CUSTOMER = 8002,
            UNVERIFIED_CREATE_CUSTOMER = 8003,
            UNVERIFIED_UPDATE_CUSTOMER = 8008
        }
    }
}
