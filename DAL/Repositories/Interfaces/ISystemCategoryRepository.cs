using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface ISystemCategoryRepository : IRepository<SystemCategory>
    {
        /// <summary>
        /// Get System Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="queryPage"></param>
        /// <param name="isAsc"></param>
        /// <param name="propertyName"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<PagingModel<SystemCategory>> GetSystemCategory(
            string id, string merchantId,
            int?[] status, string type, string search,
            int? limit, int? queryPage, bool isAsc,
            string propertyName, string include);
    }
}