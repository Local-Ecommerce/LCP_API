using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblSystemCategory
    {
        public TblSystemCategory()
        {
            TblProductCategories = new HashSet<TblProductCategory>();
        }

        public int SystemCategoryId { get; set; }
        public string SysCategoryName { get; set; }

        public virtual ICollection<TblProductCategory> TblProductCategories { get; set; }
    }
}
