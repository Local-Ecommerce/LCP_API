using System.Collections.Generic;
using System.Text;
using BLL.Services.Interfaces;
using BLL.Dtos.MoMo.CaptureWallet;
using BLL.Dtos.MoMo.IPN;
using System.Net;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using DAL.UnitOfWork;
using System.Text.Json;
using DAL.Constants;
using System.IO;
using BLL.Dtos.Exception;
using System;
using DAL.Models;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MoMoService : IMoMoService
    {
        private HttpWebRequest request;
        private HttpWebResponse response;
        private readonly ILogger _logger;
        private readonly ISecurityService _securityService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public MoMoService(ILogger logger,
            ISecurityService securityService,
            IConfiguration configuration,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IRedisService redisService)
        {
            _logger = logger;
            _securityService = securityService;
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }


        /// <summary>
        /// Create Capture Wallet
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public MoMoCaptureWalletResponse CreateCaptureWallet(MoMoCaptureWalletRequest requestData)
        {
            MoMoCaptureWalletResponse result;
            //try
            //{
            // Convert object to json string
            string jsonData = requestData.ToJson.ToString();

            // Encoding to UTF8 before pass params
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            request = (HttpWebRequest)WebRequest.Create(Endpoint.MOMO_TEST + Endpoint.MOMO_CREATE_PAYMENT);
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = byteData.Length;
            request.Timeout = (int)TimeUnit.TIMEOUT_20_SEC;
            request.ReadWriteTimeout = (int)TimeUnit.READ_WRITE_TIMEOUT;

            _logger.Information($"[MoMoService.CreateCaptureWallet] Start request with data: {jsonData}");

            // Open request stream to send data
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(byteData, 0, byteData.Length);

            // Must close stream after write data
            requestStream.Close();

            // Open response
            response = (HttpWebResponse)request.GetResponse();

            // Open response stream to receive data
            Stream responseStream = response.GetResponseStream();

            string responseString;
            using (StreamReader streamReader = new(responseStream, Encoding.UTF8))
            {
                responseString = streamReader.ReadToEnd();
            }

            // Must close response and its stream after receive data
            response.Close();
            responseStream.Close();

            _logger.Information($"[MoMoService.CreateCaptureWallet] End request with data: {responseString}");

            result = JsonSerializer.Deserialize<MoMoCaptureWalletResponse>(responseString);

            return result;
        }


        /// <summary>
        /// Process IPN
        /// </summary>
        /// <param name="momoIPNRequest"></param>
        /// <returns></returns>
        public async Task ProcessIPN(MoMoIPNRequest momoIPNRequest)
        {
            // Validate signature
            List<string> ignoreFields = new List<string>() { "Signature", "PartnerName", "StoreId", "Lang" };

            string merchantSignature = _securityService.GetSignature(momoIPNRequest, ignoreFields,
                _configuration.GetValue<string>("MoMo:AccessKey"), _configuration.GetValue<string>("MoMo:SecretKey"));

            _logger.Information("[MoMoService.ProcessIPN] MoMo - Merchant signature: " +
                $"{momoIPNRequest.Signature} - {merchantSignature}");

            if (!merchantSignature.Equals(momoIPNRequest.Signature))
            {
                _logger.Error("[MoMoService.ProcessIPN] Signature not match!");

                throw new BusinessException(MoMoStatus.MOMO_IPN_SIGNATURE_NOT_MATCH.ToString(), (int)MoMoStatus.MOMO_IPN_SIGNATURE_NOT_MATCH);

            }

            // update payment
            await UpdatePaymentResult(momoIPNRequest);
        }


        /// <summary>
        /// Update Payment Result
        /// </summary>
        /// <param name="momoIPNRequest"></param>
        public async Task UpdatePaymentResult(MoMoIPNRequest momoIPNRequest)
        {
            try
            {
                List<string> orderIds = _redisService.GetList<string>(momoIPNRequest.OrderId);
                foreach (var orderId in orderIds)
                {
                    Payment payment = await _unitOfWork.Payments.FindAsync(p => p.OrderId.Equals(orderId));
                    payment.TransactionId = momoIPNRequest.TransId;
                    payment.ResultCode = momoIPNRequest.ResultCode;
                    payment.Status = momoIPNRequest.ResultCode == 0 ? (int)PaymentStatus.PAID : (int)PaymentStatus.FAILED;

                    _unitOfWork.Payments.Update(payment);
                }

                await _unitOfWork.SaveChangesAsync();

                //delete from Redis
                _redisService.RemoveList(momoIPNRequest.OrderId);
            }
            catch (Exception e)
            {
                _logger.Error("[MoMoService.UpdatePaymentResult()]: " + e.Message);
                throw;
            }
        }
    }
}
