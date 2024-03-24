using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models {
	public partial class RefreshToken {
		public string Token { get; set; }
		public string AccessToken { get; set; }
		public bool? IsRevoked { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public string AccountId { get; set; }

		public virtual Account Account { get; set; }
	}
}
