using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Poi
    {
        public string PoiId { get; set; }
        public DateTime? RealeaseDate { get; set; }
        public string Text { get; set; }
        public int? Status { get; set; }
        public string MarketManagerId { get; set; }
        public string AparmentId { get; set; }

        public virtual Apartment Aparment { get; set; }
        public virtual MarketManager MarketManager { get; set; }
    }
}
