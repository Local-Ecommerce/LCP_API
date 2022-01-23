using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IMenuRepository : IRepository<Menu>
    {
        /// <summary>
        /// Get All Menus Include Resident
        /// </summary>
        /// <returns></returns>
        Task<List<Menu>> GetAllMenusIncludeResident();


        /// <summary>
        /// Get Menu Include Resident By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Menu> GetMenuIncludeResidentById(string id);


        /// <summary>
        /// Get Menu By Resident Id
        /// </summary>
        /// <param name="residentId"></param>
        /// <returns></returns>
        Task<List<Menu>> GetMenusByResidentId(string residentId);
    }
}