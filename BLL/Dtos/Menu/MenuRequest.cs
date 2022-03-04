using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.Menu
{
    [Serializable]
    public class MenuRequest
    {
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }

        [RegularExpression(@"^(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)$")]
        public string TimeStart { get; set; }

        [RegularExpression(@"^(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)$")]
        public string TimeEnd { get; set; }
        public string RepeatDate { get; set; }
        public string MerchantStoreId { get; set; }
    }
}
