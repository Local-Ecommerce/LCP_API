using AutoMapper;
using BLL.Constants;
using BLL.Dtos;
using BLL.Dtos.Exception;
using BLL.Dtos.Payment;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUtilService _utilService;
        private const string PREFIX = "PM_";

        public PaymentService(IUnitOfWork unitOfWork,
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
        /// Create Payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentResponse>> CreatePayment(PaymentRequest paymentRequest)
        {
            Payment Payment = _mapper.Map<Payment>(paymentRequest);

            try
            {
                Payment.PaymentId = _utilService.CreateId(PREFIX);
                Payment.DateTime = DateTime.Now;
                Payment.Status = (int)PaymentStatus.ACTIVE_PAYMENT;

                _unitOfWork.Payments.Add(Payment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.CreatePayment()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<PaymentResponse>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }
            //Create Response
            PaymentResponse PaymentResponse = _mapper.Map<PaymentResponse>(Payment);

            return new BaseResponse<PaymentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = PaymentResponse
            };
        }


        /// <summary>
        /// Delete Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentResponse>> DeletePaymentById(string id)
        {
            //Check id
            Payment payment;
            try
            {
                payment = await _unitOfWork.Payments.FindAsync(local => local.PaymentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.DeletePaymentById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)PaymentStatus.PAYMENT_NOT_FOUND,
                    ResultMessage = PaymentStatus.PAYMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Delete Payment
            try
            {
                payment.Status = (int)PaymentStatus.INACTIVE_PAYMENT;

                _unitOfWork.Payments.Update(payment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.DeletePaymentById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            PaymentResponse PaymentResponse = _mapper.Map<PaymentResponse>(payment);

            return new BaseResponse<PaymentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = PaymentResponse
            };
        }


        /// <summary>
        /// Get Payment By Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<PaymentResponse>>> GetPaymentByDate(DateTime date)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payment from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(payment => payment.DateTime.Value.Date == date.Date);

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByDate()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)PaymentStatus.PAYMENT_NOT_FOUND,
                    ResultMessage = PaymentStatus.PAYMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<PaymentResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentResponses
            };
        }


        /// <summary>
        /// Get Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentResponse>> GetPaymentById(string id)
        {
            PaymentResponse paymentReponse;

            //Get Payment from DB

            try
            {
                Payment payment = await _unitOfWork.Payments.FindAsync(local => local.PaymentId.Equals(id));

                paymentReponse = _mapper.Map<PaymentResponse>(payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)PaymentStatus.PAYMENT_NOT_FOUND,
                    ResultMessage = PaymentStatus.PAYMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<PaymentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentReponse
            };
        }


        /// <summary>
        /// Get Payment By Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<PaymentResponse>>> GetPaymentByOrderId(string orderId)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payments from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(Payment => Payment.OrderId.Equals(orderId));

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByOrderId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)PaymentStatus.PAYMENT_NOT_FOUND,
                    ResultMessage = PaymentStatus.PAYMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<PaymentResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentResponses
            };
        }


        /// <summary>
        /// Get Payment By Payment Amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<PaymentResponse>>> GetPaymentByPaymentAmount(string amount)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payments from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(Payment => Payment.PaymentAmount.Equals(amount));

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByPaymentAmount()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)PaymentStatus.PAYMENT_NOT_FOUND,
                    ResultMessage = PaymentStatus.PAYMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<PaymentResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentResponses
            };
        }


        /// <summary>
        /// Get Payment By Payment Method Id
        /// </summary>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<List<PaymentResponse>>> GetPaymentByPaymentMethodId(string paymentMethodId)
        {
            List<PaymentResponse> paymentResponses;

            //Get Payments from DB

            try
            {
                List<Payment> Payment = await _unitOfWork.Payments.FindListAsync(Payment => Payment.PaymentMethodId.Equals(paymentMethodId));

                paymentResponses = _mapper.Map<List<PaymentResponse>>(Payment);
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.GetPaymentByPaymentMethodId()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)PaymentStatus.PAYMENT_NOT_FOUND,
                    ResultMessage = PaymentStatus.PAYMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            return new BaseResponse<List<PaymentResponse>>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = paymentResponses
            };
        }


        /// <summary>
        /// Update Payment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<BaseResponse<PaymentResponse>> UpdatePaymentById(string id, PaymentRequest paymentRequest)
        {
            Payment payment;
            try
            {
                payment = await _unitOfWork.Payments.FindAsync(local => local.PaymentId.Equals(id));
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.UpdatePaymentById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)PaymentStatus.PAYMENT_NOT_FOUND,
                    ResultMessage = PaymentStatus.PAYMENT_NOT_FOUND.ToString(),
                    Data = default
                });
            }

            //Update Payment to DB
            try
            {
                payment = _mapper.Map(paymentRequest, payment);

                _unitOfWork.Payments.Update(payment);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.Error("[PaymentService.UpdatePaymentById()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<Payment>
                {
                    ResultCode = (int)CommonResponse.ERROR,
                    ResultMessage = CommonResponse.ERROR.ToString(),
                    Data = default
                });
            }

            //Create Response
            PaymentResponse PaymentResponse = _mapper.Map<PaymentResponse>(payment);

            return new BaseResponse<PaymentResponse>
            {
                ResultCode = (int)CommonResponse.SUCCESS,
                ResultMessage = CommonResponse.SUCCESS.ToString(),
                Data = PaymentResponse
            };
        }
    }
}
