using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblCollection
    {
        public TblCollection()
        {
            TblCollectionMappings = new HashSet<TblCollectionMapping>();
        }

        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? MerchantId { get; set; }

        public virtual TblMerchant Merchant { get; set; }
        public virtual ICollection<TblCollectionMapping> TblCollectionMappings { get; set; }
    }
}
