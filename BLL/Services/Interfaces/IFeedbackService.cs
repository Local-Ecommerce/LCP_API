using System;
using System.Threading.Tasks;
using BLL.Dtos.Feedback;
using DAL.Models;

namespace BLL.Services.Interfaces {
	public interface IFeedbackService {
		/// <summary>
		/// Create Feedback
		/// </summary>
		/// <param name="request"></param>
		/// <param name="residentId"></param>
		/// <returns></returns>
		Task<FeedbackResponse> CreateFeedback(FeedbackRequest request, string residentId);

		/// <summary>
		/// Get Feedback
		/// </summary>
		/// <param name="request"></param>
		/// <param name="residentSendRequest"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		Task<PagingModel<FeedbackResponse>> GetFeedback(GetFeedbackRequest request, string role, string residentSendRequest);

		/// <summary>
		/// Read Feedback
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<FeedbackResponse> ReadFeedback(string id);
	}
}