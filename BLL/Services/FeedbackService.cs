using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Dtos.Feedback;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using DAL.UnitOfWork;

namespace BLL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private readonly IFirebaseService _firebaseService;
        private readonly IUnitOfWork _unitOfWork;
        private const string PREFIX = "FB_";
        private const string TYPE = "Feedback";

        public FeedbackService(
            ILogger logger,
            IMapper mapper,
            IUtilService utilService,
            IFirebaseService firebaseService,
            IUnitOfWork unitOfWork
        )
        {
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
            _firebaseService = firebaseService;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Create Feedback
        /// </summary>
        /// <param name="request"></param>
        /// <param name="residentId"></param>
        /// <returns></returns>
        public async Task<FeedbackResponse> CreateFeedback(FeedbackRequest request, string residentId)
        {
            Feedback feedback = _mapper.Map<Feedback>(request);

            try
            {
                feedback.FeedbackId = _utilService.CreateId(PREFIX);
                feedback.FeedbackDate = _utilService.CurrentTimeInVietnam();
                feedback.Image = _firebaseService
                        .UploadFilesToFirebase(request.Image, TYPE, feedback.FeedbackId, "Image", 0)
                        .Result;
                feedback.ResidentId = residentId;

                _unitOfWork.Feedbacks.Add(feedback);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[FeedbackService.CreateFeedback()]: " + e.Message);

                throw;
            }

            return _mapper.Map<FeedbackResponse>(feedback);
        }


        /// <summary>
        /// Get Feedback
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="residentId"></param>
        /// <param name="residentSendRequest"></param>
        /// <param name="role"></param>
        /// <param name="rating"></param>
        /// <param name="date"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<PagingModel<FeedbackResponse>> GetFeedback(
            string id, string productId, string residentId, string residentSendRequest,
            string role, double? rating, DateTime? date,
            int? limit, int? page, string sort, string[] include)
        {
            PagingModel<Feedback> feedback;
            string propertyName = default;
            bool isAsc = false;
            string apartmentId = null;

            if (role.Equals(ResidentType.CUSTOMER))
                residentId = residentSendRequest;

            if (role.Equals(ResidentType.MARKET_MANAGER))
            {
                apartmentId = (await _unitOfWork.Residents.FindAsync(r => r.ResidentId.Equals(residentSendRequest)))
                                .ApartmentId;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            //include
            for (int i = 0; i < include.Length; i++)
            {
                include[i] = !string.IsNullOrEmpty(include[i]) ? _utilService.UpperCaseFirstLetter(include[i]) : null;
            }

            try
            {
                feedback = await _unitOfWork.Feedbacks
                    .GetFeedback(id, productId, residentId, residentSendRequest,
                        role, apartmentId, rating, date, limit, page, isAsc, propertyName, include);
            }
            catch (Exception e)
            {
                _logger.Error("[FeedbackService.GetFeedback()]" + e.Message);
                throw;
            }

            return new PagingModel<FeedbackResponse>
            {
                List = _mapper.Map<List<FeedbackResponse>>(feedback.List),
                Page = feedback.Page,
                LastPage = feedback.LastPage,
                Total = feedback.Total,
            };
        }
    }
}