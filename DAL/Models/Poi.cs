using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Poi
    {
        public string PoiId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool? Priority { get; set; }
        public string Type { get; set; }
        public int? Status { get; set; }
        public string ResidentId { get; set; }
        public string ApartmentId { get; set; }

        public virtual Apartment Apartment { get; set; }
        public virtual Resident Resident { get; set; }
    }
}
