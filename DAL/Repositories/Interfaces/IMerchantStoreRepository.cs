using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IMerchantStoreRepository : IRepository<MerchantStore>
    {
        /// <summary>
        /// Get All Merchant Stores Include Resident And Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<MerchantStore>> GetAllMerchantStoresIncludeResidentAndApartment();


        /// <summary>
        /// Get Merchant Store Include Resident By Id
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        Task<MerchantStore> GetMerchantStoreIncludeResidentById(string merchantStoreId);


        /// <summary>
        /// Get Unverified Merchant Store Include Resident
        /// </summary>
        /// <returns></returns>
        Task<List<MerchantStore>> GetUnverifiedMerchantStoreIncludeResident();


        /// <summary>
        /// Get Menus By Store Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MerchantStore> GetMenusByStoreId(string id);
    }
}