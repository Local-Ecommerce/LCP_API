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
        /// Get Merchant Store Include Resident By Unvertified Status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<MerchantStore>> GetPendingMerchantStoreIncludeResidentByUnvertifiedStatus();
    }
}