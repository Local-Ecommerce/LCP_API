using Microsoft.AspNetCore.Mvc;
using System;

namespace BLL.Dtos.Feedback {
	public class FeedbackRequest {

		public string FeedbackDetail { get; set; }
		public string[] Image { get; set; }
		public string ProductId { get; set; }
	}

	public class GetFeedbackRequest : PagingDto {
		[FromQuery(Name = "id")]
		public string ID { get; set; }

		[FromQuery(Name = "productid")]
		public string ProductId { get; set; }

		[FromQuery(Name = "residentid")]
		public string ResidentId { get; set; }

		[FromQuery(Name = "rating")]
		public double? Rating { get; set; }

		[FromQuery(Name = "date")]
		public DateTime? Date { get; set; }

		public override string ToString() {
			return $"ID: {ID}, ProductId: {ProductId}, ResidentId: {ResidentId}, Rating: {Rating}, Date: {Date}. {base.ToString()}";
		}
	}
}