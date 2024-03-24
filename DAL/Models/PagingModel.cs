using System.Collections.Generic;

namespace DAL.Models {
	public class PagingModel<T> {
		public List<T> List { get; set; }
		public int Page { get; set; }
		public int Total { get; set; }
		public int LastPage { get; set; }
	}

	public abstract class PagingRequest {
		public int? Limit { get; set; }
		public int? Page { get; set; }
		public string Search { get; set; }
		public string OrderBy { get; set; }
		public bool IsAsc { get; set; }
		public string[] Include { get; set; } = null;
		public string PropertyName { get; set; }
		public int? QueryPage { get; set; }
	}
}
