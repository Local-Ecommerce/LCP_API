using AutoMapper;
using DAL.Constants;
using BLL.Dtos.Exception;
using BLL.Dtos.PaymentMethod;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PMM_";

        public PaymentMethodService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _utilService = utilService;
        }


        /// <summary>
        /// Create Payment Method
        /// </summary>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        public async Task<PaymentMethodResponse> CreatePaymentMethod(PaymentMethodRequest paymentMethodRequest)
        {
            //biz rule

            //Store PaymentMethod To Dabatabase
            PaymentMethod paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodRequest);

            try
            {
                paymentMethod.PaymentMethodId = _utilService.CreateId(PREFIX);
                paymentMethod.Status = (int)PaymentMethodStatus.ACTIVE_PAYMENT_METHOD;

                _unitOfWork.PaymentMethods.Add(paymentMethod);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.CreatePaymentMethod()]: " + e.Message);

                throw;
            }

            return _mapper.Map<PaymentMethodResponse>(paymentMethod);
        }


        /// <summary>
        /// Delete Payment Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePaymentMethod(string id)
        {
            //biz rule

            //Check id
            PaymentMethod paymentMethod;
            try
            {
                paymentMethod = await _unitOfWork.PaymentMethods.FindAsync(pmm => pmm.PaymentMethodId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.DeletePaymentMethod()]: " + e.Message);

                throw new EntityNotFoundException(typeof(PaymentMethod), id);
            }

            //Delete PaymentMethod
            try
            {
                paymentMethod.Status = (int)PaymentMethodStatus.DELETED_PAYMENT_METHOD;

                _unitOfWork.PaymentMethods.Update(paymentMethod);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.DeletePaymentMethod()]: " + e.Message);

                throw;
            }
        }


        /// <summary>
        /// Update Payment Method By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        public async Task<PaymentMethodResponse> UpdatePaymentMethodById(string id, PaymentMethodRequest paymentMethodRequest)
        {
            //biz ruie

            //Check id
            PaymentMethod paymentMethod;
            try
            {
                paymentMethod = await _unitOfWork.PaymentMethods.FindAsync(pmm => pmm.PaymentMethodId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.UpdatePaymentMethodById()]: " + e.Message);

                throw new EntityNotFoundException(typeof(PaymentMethod), id);
            }

            //Update PaymentMethod To DB
            try
            {
                paymentMethod = _mapper.Map(paymentMethodRequest, paymentMethod);
                paymentMethod.Status = (int)PaymentMethodStatus.ACTIVE_PAYMENT_METHOD;

                _unitOfWork.PaymentMethods.Update(paymentMethod);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.UpdatePaymentMethodById()]: " + e.Message);

                throw;
            }

            return _mapper.Map<PaymentMethodResponse>(paymentMethod);
        }


        /// <summary>
        /// Get Payment Method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public async Task<object> GetPaymentMethods(string id, int?[] status, int? limit, int? page, string sort)
        {
            PagingModel<PaymentMethod> paymentMethods;
            string propertyName = default;
            bool isAsc = false;

            if (!string.IsNullOrEmpty(sort))
            {
                isAsc = sort[0].ToString().Equals("+");
                propertyName = _utilService.UpperCaseFirstLetter(sort[1..]);
            }

            try
            {
                paymentMethods = await _unitOfWork.PaymentMethods.GetPaymentMethod(id, status, limit, page, isAsc, propertyName);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.GetPaymentMethod()]" + e.Message);
                throw;
            }

            return new PagingModel<PaymentMethodResponse>
            {
                List = _mapper.Map<List<PaymentMethodResponse>>(paymentMethods.List),
                Page = paymentMethods.Page,
                LastPage = paymentMethods.LastPage,
                Total = paymentMethods.Total,
            };
        }
    }
}
