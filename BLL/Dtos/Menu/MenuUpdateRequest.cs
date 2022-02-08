using System;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class MenuUpdateRequest
    {
        public string MenuName { get; set; }
        public int Status { get; set; }
    }
}
