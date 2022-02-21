using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IApartmentRepository : IRepository<Apartment>
    {
        /// <summary>
        /// Get Apartment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<Apartment>> GetApartment(
            string id, int?[] status,
            int? limit, int? queryPage,
            bool isAsc, string propertyName, string include);
    }
}