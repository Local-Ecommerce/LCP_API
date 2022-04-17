using System;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        /// <summary>
        /// Get Feedback
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="residentId"></param>
        /// <param name="residentSendRequest"></param>
        /// <param name="apartmentId"></param>
        /// <param name="role"></param>
        /// <param name="rating"></param>
        /// <param name="date"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<Feedback>> GetFeedback(
            string id, string productId, string residentId, string residentSendRequest, string role,
            string apartmentId, double? rating, DateTime? date,
            int? limit, int? queryPage,
            bool? isAsc, string propertyName, string[] include);
    }
}