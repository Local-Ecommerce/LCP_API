using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models {
	public partial class Apartment {
		public Apartment() {
			MerchantStores = new HashSet<MerchantStore>();
			News = new HashSet<News>();
			Pois = new HashSet<Poi>();
			Residents = new HashSet<Resident>();
		}

		public string ApartmentId { get; set; }
		public string ApartmentName { get; set; }
		public string Address { get; set; }
		public int? Status { get; set; }

		public virtual ICollection<MerchantStore> MerchantStores { get; set; }
		public virtual ICollection<News> News { get; set; }
		public virtual ICollection<Poi> Pois { get; set; }
		public virtual ICollection<Resident> Residents { get; set; }
	}

	public class ApartmentPagingRequest : PagingRequest {
		public string Id { get; set; }
		public int?[] Status { get; set; }
	}
}
