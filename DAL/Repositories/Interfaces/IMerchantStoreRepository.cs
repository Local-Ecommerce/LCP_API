using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IMerchantStoreRepository : IRepository<MerchantStore>
    {
        /// <summary>
        /// Get All Merchant Stores InClude Merchant And Apartment
        /// </summary>
        /// <returns></returns>
        Task<List<MerchantStore>> GetAllMerchantStoresInCludeMerchantAndApartment();
    }
}