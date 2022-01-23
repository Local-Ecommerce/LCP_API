using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Dtos.MerchantStore
{
    public class MerchantStoreUpdateRequest
    {
        public string StoreName { get; set; }
        public int? Status { get; set; }
        public string ApartmentId { get; set; }
    }
}