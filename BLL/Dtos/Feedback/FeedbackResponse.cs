using System;

namespace BLL.Dtos.Feedback
{
    public class FeedbackResponse
    {
        public string FeedbackId { get; set; }
        public string FeedbackDetail { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public string Image { get; set; }
        public string ResidentId { get; set; }
        public string ProductId { get; set; }
    }
}