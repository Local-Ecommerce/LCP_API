using AutoMapper;
using DAL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.News;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "NS_";

        public NewsService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }

        /// <summary>
        /// Create news
        /// </summary>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> CreateNews(NewsRequest newsRequest)
        {
            News news = _mapper.Map<News>(newsRequest);

            try
            {
                news.NewsId = _utilService.CreateId(PREFIX);
                news.ReleaseDate = DateTime.Now;
                news.Status = (int)NewsStatus.ACTIVE_NEWS;

                _unitOfWork.News.Add(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.CreateNews()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<NewsResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }
            //Create Response
            NewsResponse newsResponse = _mapper.Map<NewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponse
            };
        }

        /// <summary>
        /// Get News By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<ExtendNewsResponse>> GetNewsById(string id)
        {
            ExtendNewsResponse newsReponse;

            //Get News from DB

            try
            {
                News news = await _unitOfWork.News.GetNewsIncludeResidentAndApartmentByNewsId(id);

                newsReponse = _mapper.Map<ExtendNewsResponse>(news);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<ExtendNewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsReponse
            };
        }

        /// <summary>
        /// Get News By Release Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ExtendNewsResponse>>> GetNewsByReleaseDate(DateTime date)
        {
            List<ExtendNewsResponse> ExtendNewsResponses;

            //Get News from DB

            try
            {
                List<News> news = await _unitOfWork.News.FindListAsync(news => news.ReleaseDate.Value.Date == date.Date);

                ExtendNewsResponses = _mapper.Map<List<ExtendNewsResponse>>(news);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsByReleasedDate()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<ExtendNewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendNewsResponses
            };
        }

        /// <summary>
        /// GetNewsByApartmentId
        /// </summary>
        /// <param name="apatrmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<ExtendNewsResponse>>> GetNewsByAparmentId(string apatrmentId)
        {
            List<ExtendNewsResponse> ExtendNewsResponses;

            //Get ApartmentId from DB

            try
            {
                List<News> news = await _unitOfWork.News.FindListAsync(news => news.ApartmentId.Equals(apatrmentId));

                ExtendNewsResponses = _mapper.Map<List<ExtendNewsResponse>>(news);
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsByAparmentId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<ExtendNewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = ExtendNewsResponses
            };
        }

        /// <summary>
        ///  Update News By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsUpdateRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> UpdateNewsById(string id, NewsUpdateRequest newsUpdateRequest)
        {
            News news;
            try
            {
                news = await _unitOfWork.News.FindAsync(local => local.NewsId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.UpdateNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update News to DB
            try
            {
                news = _mapper.Map(newsUpdateRequest, news);

                _unitOfWork.News.Update(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.UpdateNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            NewsResponse newsResponse = _mapper.Map<NewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponse
            };
        }

        /// <summary>
        /// Delete News
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> DeleteNewsById(string id)
        {
            //Check id
            News news;
            try
            {
                news = await _unitOfWork.News.FindAsync(local => local.NewsId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.DeleteNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                    ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Delete News
            try
            {
                news.Status = (int)NewsStatus.INACTIVE_NEWS;

                _unitOfWork.News.Update(news);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.DeleteNewsById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            NewsResponse newsResponse = _mapper.Map<ExtendNewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponse
            };
        }


        /// <summary>
        /// Get News By Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<ExtendNewsResponse>>> GetNewsByStatus(int status)
        {
            List<ExtendNewsResponse> newsList = null;

            //get News from database
            try
            {
                newsList = _mapper.Map<List<ExtendNewsResponse>>(
                    await _unitOfWork.News.FindListAsync(mar => mar.Status == status));
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetNewsByStatus()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendNewsResponse>
                    {
                        ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                        ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<ExtendNewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsList
            };
        }

        /// <summary>
        /// Get All News
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<ExtendNewsResponse>>> GetAllNews()
        {
            List<News> news;
            try
            {
                news = await _unitOfWork.News.GetAllNewsIncludeApartmentAndResident();
            }
            catch (Exception e)
            {
                _logger.Error("[NewsService.GetAllNews()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<ExtendNewsResponse>
                    {
                        ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                        ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            List<ExtendNewsResponse> newsList = _mapper.Map<List<ExtendNewsResponse>>(news);

            return new BaseResponse<List<ExtendNewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsList
            };
        }
    }
}
