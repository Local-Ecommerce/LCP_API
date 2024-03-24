using System;

namespace BLL.Dtos.Apartment {
	[Serializable]
	public class ApartmentResponse {
		public string ApartmentId { get; set; }
		public string ApartmentName { get; set; }
		public string Address { get; set; }
		public int? Status { get; set; }
	}
}
