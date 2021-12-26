using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.News;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "News";

        public NewsService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
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
                news.NewsId = _utilService.Create16Alphanumeric();
                news.ReleaseDate = DateTime.Now;
                news.Status = (int)NewsStatus.ACTIVE_NEWS;

                _unitOfWork.Repository<News>().Add(news);

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

            //Store News to Redis
            _redisService.StoreToList(CACHE_KEY, newsResponse, new Predicate<NewsResponse>(a => a.NewsId == newsResponse.NewsId));

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
        public async Task<BaseResponse<NewsResponse>> GetNewsById(string id)
        {
            NewsResponse newsReponse = null;
            //Get News from Redis
            newsReponse = _redisService.GetList<NewsResponse>(CACHE_KEY).Find(local => local.NewsId.Equals(id));

            //Get News from DB
            if(newsReponse is null)
            {
                try
                {
                    News news = await _unitOfWork.Repository<News>().FindAsync(local => local.NewsId.Equals(id));

                    newsReponse = _mapper.Map<NewsResponse>(news);
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
            }

            return new BaseResponse<NewsResponse>
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
        public async Task<BaseResponse<List<NewsResponse>>> GetNewsByReleaseDate(DateTime date)
        {
            List<NewsResponse> newsResponses = null;

            //Get News from Redis
            newsResponses = _redisService.GetList<NewsResponse>(CACHE_KEY)
                .Where(news => _utilService.CompareDateTimes(news.ReleaseDate, date))
                .ToList();

            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(newsResponses))
            {
                try
                {
                    List<News> news = await _unitOfWork.Repository<News>().FindListAsync(news => news.ReleaseDate.Equals(date));

                    newsResponses = _mapper.Map<List<NewsResponse>>(news);
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
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponses
            };
        }

        /// <summary>
        /// GetNewsByApartmentId
        /// </summary>
        /// <param name="apatrmentId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<NewsResponse>>> GetNewsByAparmentId(string apatrmentId)
        {
            List<NewsResponse> newsResponses = null;

            //Get News from Redis
            newsResponses = _redisService.GetList<NewsResponse>(CACHE_KEY).Where(news => news.ApartmentId.Equals(apatrmentId)).ToList();

            //Get ApartmentId from DB
            if(_utilService.IsNullOrEmpty(newsResponses))
            {
                try
                {
                    List<News> news = await _unitOfWork.Repository<News>().FindListAsync(news => news.ApartmentId.Equals(apatrmentId));

                    newsResponses = _mapper.Map<List<NewsResponse>>(news);
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
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponses
            };
        }

        /// <summary>
        /// GetNewsByMarketManagerId
        /// </summary>
        /// <param name="MarketManagerId"></param>
        /// <returns></returns>
        public async Task<BaseResponse<List<NewsResponse>>> GetNewsByMarketManagerId(string MarketManagerId)
        {
            List<NewsResponse> newsResponses = null;

            //Get News from Redis
            newsResponses = _redisService.GetList<NewsResponse>(CACHE_KEY)
                .Where(news => news.MarketManagerId.Equals(MarketManagerId))
                .ToList();

            //Get ApartmentId from DB
            if (_utilService.IsNullOrEmpty(newsResponses))
            {
                try
                {
                    List<News> news = await _unitOfWork.Repository<News>()
                        .FindListAsync(news => news.MarketManagerId.Equals(MarketManagerId));

                    newsResponses = _mapper.Map<List<NewsResponse>>(news);
                }
                catch (Exception e)
                {
                    _logger.Error("[NewsService.GetNewsByMarketManagerId()]: " + e.Message);

                    throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<News>
                    {
                        ResultCode = (int)NewsStatus.NEWS_NOT_FOUND,
                        ResultMessage = NewsStatus.NEWS_NOT_FOUND.ToString(),
                        Data = default
                    });
                }
            }

            return new BaseResponse<List<NewsResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponses
            };
        }

        /// <summary>
        ///  UpdateNewsById
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse<NewsResponse>> UpdateNewsById(string id, NewsRequest newsRequest)
        {
            News news;
            try
            {
                news = await _unitOfWork.Repository<News>().FindAsync(local => local.NewsId.Equals(id));
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
                news = _mapper.Map(newsRequest, news);

                _unitOfWork.Repository<News>().Update(news);

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

            //Store to Redis
            _redisService.StoreToList(CACHE_KEY, newsResponse, new Predicate<NewsResponse>(a => a.NewsId == newsResponse.NewsId));

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
                news = await _unitOfWork.Repository<News>().FindAsync(local => local.NewsId.Equals(id));
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

                _unitOfWork.Repository<News>().Update(news);

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
            NewsResponse newsResponse = _mapper.Map<NewsResponse>(news);

            //Store News to Redis
            _redisService.StoreToList<NewsResponse>(CACHE_KEY, newsResponse,
                new Predicate<NewsResponse>(a => a.NewsId == newsResponse.NewsId));

            return new BaseResponse<NewsResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = newsResponse
            };
        }
    }
}
