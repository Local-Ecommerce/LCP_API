using System;

namespace BLL.Dtos.Feedback
{
    public class FeedbackRequest
    {

        public string FeedbackDetail { get; set; }
        public string[] Image { get; set; }
        public string ProductId { get; set; }
    }
}