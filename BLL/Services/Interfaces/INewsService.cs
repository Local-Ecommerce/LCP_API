using BLL.Dtos.News;
using System;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface INewsService
    {
        /// <summary>
        /// Create news
        /// </summary>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        Task<NewsResponse> CreateNews(NewsRequest newsRequest);


        /// <summary>
        /// Get News
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="date"></param>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<object> GetNews(
            string id, string apartmentId,
            DateTime date, string search, int?[] status,
            int? limit, int? page,
            string sort, string[] include);


        /// <summary>
        /// update news
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsUpdateRequest"></param>
        /// <returns></returns>
        Task<NewsResponse> UpdateNewsById(string id, NewsUpdateRequest newsUpdateRequest);


        /// <summary>
        /// delete news
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteNewsById(string id);
    }
}
