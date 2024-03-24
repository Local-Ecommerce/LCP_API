using Microsoft.AspNetCore.Mvc;

namespace BLL.Dtos {
	public class PagingDto {

		[FromQuery(Name = "limit")]
		public int Limit { get; set; }

		[FromQuery(Name = "page")]
		public int Page { get; set; }

		[FromQuery(Name = "sort")]
		public string Sort { get; set; }

		[FromQuery(Name = "search")]
		public string[] Search { get; set; }

		[FromQuery(Name = "include")]
		public string[] Include { get; set; }

		public override string ToString() {
			return $"Limit: {Limit}, Page: {Page}, Sort: {Sort}, Search: {string.Join(", ", Search)}, Include: {string.Join(",", Include)}";
		}
	}
}
