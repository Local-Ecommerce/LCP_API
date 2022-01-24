using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.Constants;

namespace DAL.Repositories
{
    public class MerchantStoreRepository : Repository<MerchantStore>, IMerchantStoreRepository
    {
        public MerchantStoreRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get All Merchant Stores Include Resident And Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<MerchantStore>> GetAllMerchantStoresIncludeResidentAndApartment()
        {
            List<MerchantStore> merchantStores = await _context.MerchantStores
                                                        .Include(ms => ms.Apartment)
                                                        .Include(ms => ms.Resident)
                                                        .OrderByDescending(ms => ms.CreatedDate)
                                                        .ToListAsync();

            return merchantStores;
        }


        /// <summary>
        /// Get Merchant Store Include Resident By Id
        /// </summary>
        /// <param name="merchantStoreId"></param>
        /// <returns></returns>
        public async Task<MerchantStore> GetMerchantStoreIncludeResidentById(string merchantStoreId)
        {
            MerchantStore merchantStore = await _context.MerchantStores
                                                        .Where(ms => ms.MerchantStoreId.Equals(merchantStoreId))
                                                        .Include(ms => ms.Apartment)
                                                        .Include(ms => ms.Resident)
                                                        .OrderByDescending(ms => ms.CreatedDate)
                                                        .FirstOrDefaultAsync();

            return merchantStore;
        }


        /// <summary>
        /// Get Merchant Store Include Resident By Unverified Status
        /// </summary>
        /// <returns></returns>
        public async Task<List<MerchantStore>> GetPendingMerchantStoreIncludeResidentByUnverifiedStatus()
        {
            List<MerchantStore> merchantStores = await _context.MerchantStores
                                                        .Where(ms => ms.Status == (int)MerchantStoreStatus.UNVERIFIED_CREATE_MERCHANT_STORE 
                                                        || ms.Status == (int)MerchantStoreStatus.UNVERIFIED_UPDATE_MERCHANT_STORE)
                                                        .Include(ms => ms.Apartment)
                                                        .Include(ms => ms.Resident)
                                                        .OrderByDescending(ms => ms.CreatedDate)
                                                        .ToListAsync();

            return merchantStores;
        }
    }
}