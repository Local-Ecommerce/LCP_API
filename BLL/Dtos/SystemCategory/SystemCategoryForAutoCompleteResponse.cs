using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.SystemCategory
{
    public class SystemCategoryForAutoCompleteResponse
    {
        public string SystemCategoryId { get; set; }
        public string SysCategoryName { get; set; }
        public int CategoryLevel { get; set; }
    }
}
