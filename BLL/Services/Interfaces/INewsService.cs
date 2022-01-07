using BLL.Dtos;
using BLL.Dtos.News;
using System;
using System.Collections.Generic;
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
        Task<BaseResponse<NewsResponse>> CreateNews(NewsRequest newsRequest);

        /// <summary>
        /// //Get News by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<NewsResponse>> GetNewsById(string id);

        /// <summary>
        /// Get news by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BaseResponse<List<NewsResponse>>> GetNewsByReleaseDate(DateTime date);

        /// <summary>
        /// get news by apartmentid
        /// </summary>
        /// <param name="aparmentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<NewsResponse>>> GetNewsByAparmentId(string apartmentId);

        /// <summary>
        /// get news by marketmanagerid
        /// </summary>
        /// <param name="MarketManagerId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<NewsResponse>>> GetNewsByMarketManagerId(string MarketManagerId);

        /// <summary>
        /// update news
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<NewsResponse>> UpdateNewsById(string id, NewsRequest newsRequest);

        /// <summary>
        /// delete news
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<NewsResponse>> DeleteNewsById(string id);


        /// <summary>
        /// Get News By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<BaseResponse<List<NewsResponse>>> GetNewsByStatus(int status);
    }
}
