using System;
using BLL.Dtos.MerchantStore;
using BLL.Dtos.Product;
using BLL.Dtos.Resident;

namespace BLL.Dtos.Feedback
{
    public class FeedbackResponse
    {
        public string FeedbackId { get; set; }
        public string FeedbackDetail { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public string Image { get; set; }
        public bool? IsRead { get; set; }
        public string ResidentId { get; set; }
        public string ProductId { get; set; }
        public ResidentResponse Resident { get; set; }
        public RelatedProductResponse Product { get; set; }
        public MerchantStoreResponse MerchantStore { get; set; }
    }
}