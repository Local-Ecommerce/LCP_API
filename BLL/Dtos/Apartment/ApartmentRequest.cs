using Microsoft.AspNetCore.Mvc;
using System;

namespace BLL.Dtos.Apartment {
	[Serializable]
	public class ApartmentRequest {
		public string ApartmentName { get; set; }
		public string Address { get; set; }
		public int? Status { get; set; }
	}

	[Serializable]
	public class GetApartmentRequest : PagingDto {
		[FromQuery]
		public int Id { get; set; }

		[FromQuery]
		public int?[] Status { get; set; }

		public new string ToString() {
			return base.ToString() +  $"Id: {Id}, Status: {Status}";
		}
	}
}
