using System;
using BLL.Dtos.StoreMenuDetail;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class MenuRequest
    {
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
        public StoreMenuDetailRequest StoreMenuDetail { get; set; }
    }
}
