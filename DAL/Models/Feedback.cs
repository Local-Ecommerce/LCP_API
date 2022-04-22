using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Feedback
    {
        public string FeedbackId { get; set; }
        public string FeedbackDetail { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public string Image { get; set; }
        public string ResidentId { get; set; }
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Resident Resident { get; set; }
    }
}
