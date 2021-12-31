using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.PaymentMethod;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IUtilService _utilService;
        private const string CACHE_KEY = "PaymentMethod";

        public PaymentMethodService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IRedisService redisService,
            IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _utilService = utilService;
        }


        /// <summary>
        /// Create Payment Method
        /// </summary>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentMethodResponse>> CreatePaymentMethod(PaymentMethodRequest paymentMethodRequest)
        {
            //biz rule

            //Store PaymentMethod To Dabatabase
            PaymentMethod paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodRequest);

            try
            {
                paymentMethod.PaymentMethodId = _utilService.Create16Alphanumeric();
                paymentMethod.Status = (int)PaymentMethodStatus.ACTIVE_PAYMENT_METHOD;

                _unitOfWork.Repository<PaymentMethod>().Add(paymentMethod);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.CreatePaymentMethod()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<PaymentMethodResponse>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            PaymentMethodResponse paymentMethodResponse = _mapper.Map<PaymentMethodResponse>(paymentMethod);

            return new BaseResponse<PaymentMethodResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentMethodResponse
            };
        }


        /// <summary>
        /// Delete Payment Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentMethodResponse>> DeletePaymentMethod(string id)
        {
            //biz rule

            //Check id
            PaymentMethod paymentMethod;
            try
            {
                paymentMethod = await _unitOfWork.Repository<PaymentMethod>()
                                       .FindAsync(local => local.PaymentMethodId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.DeletePaymentMethod()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<PaymentMethod>
                    {
                        ResultCode = (int)PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND,
                        ResultMessage = PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            //Delete PaymentMethod
            try
            {
                paymentMethod.Status = (int)PaymentMethodStatus.DELETED_PAYMENT_METHOD;

                _unitOfWork.Repository<PaymentMethod>().Update(paymentMethod);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.DeletePaymentMethod()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<PaymentMethod>
                    {
                        ResultCode = (int)CommonResponse.ERROR,
                        ResultMessage = CommonResponse.ERROR.ToString(),
                        Data = default
                    });
            }

            //Create Response
            PaymentMethodResponse PaymentMethodResponse = _mapper.Map<PaymentMethodResponse>(paymentMethod);

            return new BaseResponse<PaymentMethodResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = PaymentMethodResponse
            };
        }


        /// <summary>
        /// Get Payment Method By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentMethodResponse>> GetPaymentMethodById(string id)
        {
            //biz rule


            PaymentMethodResponse paymentMethodResponse;

            //Get PaymentMethod From Database

            try
            {
                PaymentMethod paymentMethod = await _unitOfWork.Repository<PaymentMethod>().
                                                        FindAsync(local => local.PaymentMethodId.Equals(id));

                paymentMethodResponse = _mapper.Map<PaymentMethodResponse>(paymentMethod);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.GetPaymentMethodById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<PaymentMethodResponse>
                    {
                        ResultCode = (int)PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND,
                        ResultMessage = PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<PaymentMethodResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentMethodResponse
            };
        }


        /// <summary>
        /// Get Payment Method By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<PaymentMethodResponse>>> GetAllPaymentMethod()
        {
            //biz rule


            List<PaymentMethodResponse> paymentMethodResponses;

            //Get All PaymentMethod From Database

            try
            {
                paymentMethodResponses = _mapper.Map<List<PaymentMethodResponse>>(
                    await _unitOfWork.Repository<PaymentMethod>()
                                     .FindListAsync(pm => pm.PaymentMethodId != null));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.GetAllPaymentMethod()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<PaymentMethodResponse>
                    {
                        ResultCode = (int)PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND,
                        ResultMessage = PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND.ToString(),
                        Data = default
                    });
            }

            return new BaseResponse<List<PaymentMethodResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentMethodResponses
            };
        }


        /// <summary>
        /// Update Payment Method By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentMethodResponse>> UpdatePaymentMethodById(string id, PaymentMethodRequest paymentMethodRequest)
        {
            //biz ruie

            //Check id
            PaymentMethod paymentMethod;
            try
            {
                paymentMethod = await _unitOfWork.Repository<PaymentMethod>()
                                       .FindAsync(local => local.PaymentMethodId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.UpdatePaymentMethodById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<PaymentMethodResponse>
                {
                    ResultCode = (int)PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND,
                    ResultMessage = PaymentMethodStatus.PAYMENT_METHOD_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update PaymentMethod To DB
            try
            {
                paymentMethod = _mapper.Map(paymentMethodRequest, paymentMethod);
                paymentMethod.Status = (int)PaymentMethodStatus.ACTIVE_PAYMENT_METHOD;

                _unitOfWork.Repository<PaymentMethod>().Update(paymentMethod);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentMethodService.UpdatePaymentMethodById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<PaymentMethodResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            PaymentMethodResponse paymentMethodResponse = _mapper.Map<PaymentMethodResponse>(paymentMethod);

            return new BaseResponse<PaymentMethodResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentMethodResponse
            };
        }
    }
}
