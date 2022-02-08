using System;

namespace BLL.Dtos.Resident
{
    [Serializable]
    public class ResidentResponse
    {
        public string ResidentId { get; set; }
        public string ResidentName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int? Status { get; set; }
        public string DeliveryAddress { get; set; }
        public string ApproveBy { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AccountId { get; set; }
        public string ApartmentId { get; set; }
    }
}
