using System;
using System.Threading.Tasks;
using BLL.Dtos.Feedback;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IFeedbackService
    {
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
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="residentId"></param>
        /// <param name="residenSendRequest"></param>
        /// <param name="role"></param>
        /// <param name="rating"></param>
        /// <param name="date"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<FeedbackResponse>> GetFeedback(
            string id, string productId, string residentId, string residenSendRequest,
            string role, double? rating, DateTime? date,
            int? limit, int? page, string sort, string[] include);


        /// <summary>
        /// Read Feedback
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FeedbackResponse> ReadFeedback(string id);
    }
}