using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MerchantStoreRepository : Repository<MerchantStore>, IMerchantStoreRepository
    {
        public MerchantStoreRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get All Merchant Stores InClude Merchant And Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<MerchantStore>> GetAllMerchantStoresInCludeMerchantAndApartment()
        {
            List<MerchantStore> merchantStores = await _context.MerchantStores
                                                        .Include(ms => ms.Merchant)
                                                        .Include(ms => ms.Apartment)
                                                        .ToListAsync();

            return merchantStores;
        }
    }
}