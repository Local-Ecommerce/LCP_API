using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models {
	public partial class MerchantStore {
		public MerchantStore() {
			Menus = new HashSet<Menu>();
			Orders = new HashSet<Order>();
		}

		public string MerchantStoreId { get; set; }
		public string StoreName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string StoreImage { get; set; }
		public int? Status { get; set; }
		public int? Warned { get; set; }
		public string ResidentId { get; set; }
		public string ApartmentId { get; set; }

		public virtual Apartment Apartment { get; set; }
		public virtual Resident Resident { get; set; }
		public virtual ICollection<Menu> Menus { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
	}

	public class MerchantStorePagingRequest : PagingRequest {
		public string Id { get; set; } = null;
		public string ApartmentId { get; set; } = null;
		public string ResidentId { get; set; } = null;
		public int?[] Status { get; set; } = null;
	}
}
