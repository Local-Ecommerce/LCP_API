using System;
using System.Collections.Generic;
using BLL.Dtos.OrderDetail;
using BLL.Dtos.Resident;

namespace BLL.Dtos.Order
{
    [Serializable]
    public class OrderRequest
    {
        public string ResidentId { get; set; }
        public List<OrderDetailRequest> Products { get; set; }
        public ResidentGuest Resident { get; set; }
    }
}
