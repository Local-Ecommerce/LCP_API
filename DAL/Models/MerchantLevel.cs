using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class MerchantLevel
    {
        public MerchantLevel()
        {
            Merchants = new HashSet<Merchant>();
        }

        public string LevelId { get; set; }
        public double? PremiumFee { get; set; }

        public virtual ICollection<Merchant> Merchants { get; set; }
    }
}
