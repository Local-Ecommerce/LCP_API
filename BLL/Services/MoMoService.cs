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
using System.Net.Http;

namespace BLL.Services {
	public class MoMoService(ILogger logger,
				ISecurityService securityService,
				IConfiguration configuration,
				IUnitOfWork unitOfWork,
				IRedisService redisService) : IMoMoService {

		private HttpRequestMessage request;
		private HttpResponseMessage response;

		private readonly ILogger _logger = logger;
		private readonly ISecurityService _securityService = securityService;
		private readonly IConfiguration _configuration = configuration;
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IRedisService _redisService = redisService;

		/// <summary>
		/// Create Capture Wallet
		/// </summary>
		/// <param name="requestData"></param>
		/// <returns></returns>
		public async Task<MoMoCaptureWalletResponse> CreateCaptureWalletAsync(MoMoCaptureWalletRequest requestData) {
			MoMoCaptureWalletResponse result;
			//try
			//{
			// Convert object to json string
			string jsonData = requestData.ToJson.ToString();

			// Encoding to UTF8 before pass params
			byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

			// Create request
			HttpClient client = new() {
				Timeout = TimeSpan.FromSeconds(20),
				BaseAddress = new Uri(Endpoint.MOMO_TEST + Endpoint.MOMO_CREATE_PAYMENT),
				DefaultRequestHeaders = {
								{ "Accept", "application/json" },
								{ "Content-Type", "application/json" }
							}
			};

			request = new HttpRequestMessage(HttpMethod.Post, Endpoint.MOMO_TEST + Endpoint.MOMO_CREATE_PAYMENT) {
				Content = new ByteArrayContent(byteData),
				Version = HttpVersion.Version11,
			};

			_logger.Information($"[MoMoService.CreateCaptureWallet] Start request with data: {jsonData}");

			response = await client.SendAsync(request);

			string responseString = await response.Content.ReadAsStringAsync();

			_logger.Information($"[MoMoService.CreateCaptureWallet] End request with data: {responseString}");

			result = JsonSerializer.Deserialize<MoMoCaptureWalletResponse>(responseString);

			return result;
		}


		/// <summary>
		/// Process IPN
		/// </summary>
		/// <param name="momoIPNRequest"></param>
		/// <returns></returns>
		public async Task ProcessIPN(MoMoIPNRequest momoIPNRequest) {
			// Validate signature
			List<string> ignoreFields = new List<string>() { "Signature", "PartnerName", "StoreId", "Lang" };

			string merchantSignature = _securityService.GetSignature(momoIPNRequest, ignoreFields,
					_configuration.GetValue<string>("MoMo:AccessKey"), _configuration.GetValue<string>("MoMo:SecretKey"));

			_logger.Information("[MoMoService.ProcessIPN] MoMo - Merchant signature: " +
					$"{momoIPNRequest.Signature} - {merchantSignature}");

			if (!merchantSignature.Equals(momoIPNRequest.Signature)) {
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
		public async Task UpdatePaymentResult(MoMoIPNRequest momoIPNRequest) {
			try {
				List<string> orderIds = _redisService.GetList<string>(momoIPNRequest.OrderId);
				foreach (var orderId in orderIds) {
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
			catch (Exception e) {
				_logger.Error("[MoMoService.UpdatePaymentResult()]: " + e.Message);
				throw;
			}
		}
	}
}
