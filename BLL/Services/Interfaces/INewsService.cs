using BLL.Dtos;
using BLL.Dtos.New;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface INewsService
    {
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<BaseResponse<List<NewsResponse>>> GetNewsByReleaseDate(string date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aparmentId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<NewsResponse>>> GetNewsByAparmentId(string apartmentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MarketManagerId"></param>
        /// <returns></returns>
        Task<BaseResponse<List<NewsResponse>>> GetNewsByMarketManagerId(string MarketManagerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        Task<BaseResponse<NewsResponse>> UpdateNewsById(string id, NewsRequest newsRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<NewsResponse>> DeleteNewsById(string id);
    }
}
