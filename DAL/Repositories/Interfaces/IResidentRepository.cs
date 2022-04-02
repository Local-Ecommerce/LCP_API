using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IResidentRepository : IRepository<Resident>
    {
        /// <summary>
        /// Get Resident
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apartmentId"></param>
        /// <param name="accountId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Task<PagingModel<Resident>> GetResident(
            string id, string apartmentId, string accountId,
            int?[] status, string type, int? limit,
            int? queryPage, bool isAsc, string propertyName);
    }
}