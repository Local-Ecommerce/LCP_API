using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class TblCollectionMapping
    {
        public int CollectionId { get; set; }
        public int ProductId { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblCollection Collection { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
