using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Dtos.Dashboard;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using DAL.UnitOfWork;

namespace BLL.Services {
	public class DashboardService : IDashboardService {
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;
		private readonly IUtilService _utilService;

		public DashboardService(
				IUnitOfWork unitOfWork,
				ILogger logger,
				IUtilService utilService) {
			_unitOfWork = unitOfWork;
			_logger = logger;
			_utilService = utilService;
		}


		/// <summary>
		/// Get Dashboard For Merchant
		/// </summary>
		/// <param name="residentId"></param>
		/// <param name="days"></param>
		/// <returns></returns>
		public async Task<DashboardForMerchant> GetDashboardForMerchant(string residentId, int days) {
			DashboardForMerchant dashboardForMerchant = new();
			try {
				DateTime currentDate = _utilService.CurrentTimeInVietnam();
				DateTime previousDate = currentDate.Subtract(new TimeSpan(days, 0, 0, 0));

				List<Order> orders = await _unitOfWork.Orders.GetOrderForDashboard(days, residentId, null);

				if (!_utilService.IsNullOrEmpty(orders)) {
					dashboardForMerchant.TotalOrder = orders.Count;

					foreach (var order in orders) {
						switch (order.Status) {
							case (int)OrderStatus.COMPLETED:
								dashboardForMerchant.CompletedOrder++;
								dashboardForMerchant.TotalRevenue += order.TotalAmount.Value;
								break;
							case (int)OrderStatus.CANCELED_ORDER:
								dashboardForMerchant.CanceledOrder++;
								break;
						}
					}
				}
			}
			catch (Exception e) {
				_logger.Error("[DashboardService.GetDashboardForMerchant()]: " + e.Message);
				throw;
			}

			return dashboardForMerchant;
		}


		/// <summary>
		/// Get Dashboard For Market Manager
		/// </summary>
		/// <param name="residentId"></param>
		/// <param name="role"></param>
		/// <param name="days"></param>
		/// <returns></returns>
		public async Task<DashboardForMarketManager> GetDashboardForMarketManager(string residentId, string role, int days) {
			DashboardForMarketManager dashboardForMarketManager = new();
			try {
				DateTime currentDate = _utilService.CurrentTimeInVietnam();
				DateTime previousDate = currentDate.Subtract(new TimeSpan(days, 0, 0, 0));

				string apartmentId = role.Equals(ResidentType.MARKET_MANAGER) ? (await _unitOfWork.Residents.FindAsync(r => r.ResidentId.Equals(residentId))).ApartmentId : "";

				List<Order> orders = await _unitOfWork.Orders.GetOrderForDashboard(days, null, apartmentId);

				//get total completed and canceled order
				foreach (var order in orders) {
					switch (order.Status) {
						case (int)OrderStatus.COMPLETED:
							dashboardForMarketManager.TotalCompletedOrder++;
							break;
						case (int)OrderStatus.CANCELED_ORDER:
							dashboardForMarketManager.TotalCanceledOrder++;
							break;
					}
				}

				//get total product
				List<Product> products =
						(await _unitOfWork.Products.GetProduct(apartmentId: apartmentId, include: new string[] { "feedback" }))
						.List;

				if (!_utilService.IsNullOrEmpty(products))
					foreach (var product in products) {
						if (product.CreatedDate.Value.Date >= previousDate && product.CreatedDate <= currentDate)
							dashboardForMarketManager.TotalProduct++;

						if (!_utilService.IsNullOrEmpty(product.Feedbacks))
							//get total feedback
							foreach (var feedback in product.Feedbacks) {
								if (feedback.FeedbackDate.Value.Date >= previousDate && feedback.FeedbackDate.Value.Date <= currentDate)
									dashboardForMarketManager.TotalFeedback++;
							}
					}

				//get total store
				List<MerchantStore> stores = (await _unitOfWork.MerchantStores.GetMerchantStore(null, apartmentId, null, null, null, null, null, false, null, new string[] { "menu" })).List;

				if (!_utilService.IsNullOrEmpty(stores))
					foreach (var store in stores) {
						if (store.CreatedDate.Value.Date >= previousDate && store.CreatedDate <= currentDate)
							dashboardForMarketManager.TotalStore++;

						if (!_utilService.IsNullOrEmpty(store.Menus))
							//get total menu
							foreach (var menu in store.Menus) {
								if (menu.CreatedDate.Value.Date >= previousDate && menu.CreatedDate.Value.Date <= currentDate)
									dashboardForMarketManager.TotalMenu++;
							}
					}

			}
			catch (Exception e) {
				_logger.Error("[DashboardService.GetDashboardForMarketManager()]: " + e.Message);
				throw;
			}

			return dashboardForMarketManager;
		}
	}
}