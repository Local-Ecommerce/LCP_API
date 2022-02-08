using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Services.Interfaces;
using System.Threading.Tasks;
using BLL.Dtos.MoMo.CaptureWallet;
using BLL.Dtos.MoMo.IPN;
using System.Net;
using AutoMapper.Configuration;
using AutoMapper;
using DAL.UnitOfWork;
using System.Text.Json;
using DAL.Constants;
using System.IO;
using BLL.Dtos.Exception;
using BLL.Dtos;

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

        public MoMoService(ILogger logger, 
            ISecurityService securityService, 
            IConfiguration configuration,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _securityService = securityService;
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            string jsonData = JsonSerializer.Serialize(requestData);

            // Encoding to UTF8 before pass params
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            request = (HttpWebRequest)WebRequest.Create(Endpoint.MOMO_TEST + Endpoint.MOMO_CREATE_PAYMENT);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = byteData.Length;
            request.Timeout = (int)TimeUnit.TIMEOUT_20_SEC;
            request.ReadWriteTimeout = (int)TimeUnit.TIMEOUT_20_SEC;

            _logger.Information($"[MoMoCaptureWallet] Start request with data: {jsonData}");

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

            _logger.Information($"[MoMoCaptureWallet] End request with data: {responseString}");

            result = JsonSerializer.Deserialize <MoMoCaptureWalletResponse>(responseString);

            return result;
        }

        public MoMoIPNResponse ProcessIPN(MoMoIPNRequest momoIPNRequest)
        {
            // Validate signature
            List<string> ignoreFields = new List<string>() { "signature", "partnerName", "storeId", "lang" };
            string rawData = _securityService.GetRawDataSignature(momoIPNRequest, ignoreFields);
            rawData = "accessKey=" + _configuration.GetValue<string>("MoMo:AccessKey") + "&" + rawData;

            string merchantSignature = _securityService.SignHmacSHA256(rawData, _configuration.GetValue<string>("MoMo:SecretKey"));

            _logger.Information($"[MoMo IPN] MoMo - Merchant signature: {momoIPNRequest.Signature} - {merchantSignature}");

            if (!merchantSignature.Equals(momoIPNRequest.Signature))
            {
                _logger.Error("[MoMoIPN] Signature not match!");

                throw new HttpStatusException(HttpStatusCode.OK, new BaseResponse<MoMoIPNResponse>
                {
                    ResultCode = (int)MoMoStatus.MOMO_IPN_SIGNATURE_NOT_MATCH,
                    ResultMessage = MoMoStatus.MOMO_IPN_SIGNATURE_NOT_MATCH.ToString()
                });
            }

            // update data donate and response to client
            SendMomoPaymentResponseToClient(momoIPNRequest);

            //response to MoMo
            MoMoIPNResponse momoIPNResponse = _mapper.Map<MoMoIPNResponse>(momoIPNRequest);
            momoIPNResponse.Signature = _securityService.GetRawDataSignature(momoIPNResponse, ignoreFields);

            return momoIPNResponse;
        }


        /// <summary>
        /// Send Momo Payment Response To Client
        /// </summary>
        /// <param name="momoIPNRequest"></param>
        public void SendMomoPaymentResponseToClient(MoMoIPNRequest momoIPNRequest)
        {
            //not now dude
        }
    }
}
