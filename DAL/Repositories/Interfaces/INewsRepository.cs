using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface INewsRepository : IRepository<News>
    {
        /// <summary>
        /// Get News
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<News>> GetNews(
            string id, string apartmentId, string type,
            DateTime date, string search, int?[] status,
            int? limit, int? queryPage,
            List<string> sort, string[] include);
    }
}