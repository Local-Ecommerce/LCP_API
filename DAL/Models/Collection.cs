using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Collection
    {
        public Collection()
        {
            CollectionMappings = new HashSet<CollectionMapping>();
        }

        public string CollectionId { get; set; }
        public string CollectionName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }
        public string MerchantId { get; set; }

        public virtual Merchant Merchant { get; set; }
        public virtual ICollection<CollectionMapping> CollectionMappings { get; set; }
    }
}
