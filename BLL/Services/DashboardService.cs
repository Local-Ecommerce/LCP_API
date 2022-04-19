using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Dtos.Dashboard;
using BLL.Services.Interfaces;
using DAL.Constants;
using DAL.Models;
using DAL.UnitOfWork;

namespace BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUtilService _utilService;

        public DashboardService(
            IUnitOfWork unitOfWork,
            ILogger logger,
            IUtilService utilService)
        {
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
        public async Task<DashboardForMerchant> GetDashboardForMerchant(string residentId, int days)
        {
            DashboardForMerchant dashboardForMerchant = new();
            try
            {
                DateTime currentDate = _utilService.CurrentTimeInVietnam();
                DateTime previousDate = currentDate.Subtract(new TimeSpan(-days, 0, 0, 0));

                List<Order> orders = await _unitOfWork.Orders.FindListAsync(o => o.ResidentId.Equals(residentId)
                    && o.UpdatedDate.Value.Date <= currentDate.Date && o.UpdatedDate.Value.Date >= previousDate.Date);

                dashboardForMerchant.TotalOrder = orders.Count;
                foreach (var order in orders)
                {
                    switch (order.Status)
                    {
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
            catch (Exception e)
            {
                _logger.Error("[DashboardService.GetDashboardForMerchant()]: " + e.Message);
                throw;
            }

            return dashboardForMerchant;
        }
    }
}