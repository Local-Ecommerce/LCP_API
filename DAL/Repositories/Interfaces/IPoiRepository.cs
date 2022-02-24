using System;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IPoiRepository : IRepository<Poi>
    {
        /// <summary>
        /// Get Poi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="date"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<Poi>> GetPoi(
            string id, string apartmentId,
            DateTime date, string title, string text,
             int?[] status, int? limit, int? queryPage,
            bool isAsc, string propertyName, string[] include);
    }
}