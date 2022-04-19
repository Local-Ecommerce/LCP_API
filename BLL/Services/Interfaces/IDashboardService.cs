using System.Threading.Tasks;
using BLL.Dtos.Dashboard;

namespace BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        /// <summary>
        /// Get Dashboard For Merchant
        /// </summary>
        /// <param name="residentId"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        Task<DashboardForMerchant> GetDashboardForMerchant(string residentId, int days);
    }
}