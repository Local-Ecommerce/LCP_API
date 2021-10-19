using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblMerchantLevel
    {
        public TblMerchantLevel()
        {
            TblMerchants = new HashSet<TblMerchant>();
        }

        public int LevelId { get; set; }
        public double? PremiumFee { get; set; }

        public virtual ICollection<TblMerchant> TblMerchants { get; set; }
    }
}
