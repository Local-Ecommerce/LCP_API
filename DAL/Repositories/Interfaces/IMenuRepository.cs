using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IMenuRepository : IRepository<Menu>
    {
        /// <summary>
        /// Get All Menus Include Merchant
        /// </summary>
        /// <returns></returns>
        Task<List<Menu>> GetAllMenusIncludeMerchant();
    }
}