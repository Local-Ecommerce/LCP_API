using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class MerchantStoreRepository : Repository<MerchantStore>, IMerchantStoreRepository
    {
        public MerchantStoreRepository(LoichDBContext context) : base(context) { }


        /// <summary>
        /// Get All Merchant Stores InClude Merchant And Apartment
        /// </summary>
        /// <returns></returns>
        public async Task<List<MerchantStore>> GetAllMerchantStoresInCludeApartment()
        {
            List<MerchantStore> merchantStores = await _context.MerchantStores
                                                        .Include(ms => ms.Apartment)
                                                        .OrderByDescending(ms => ms.CreatedDate)
                                                        .ToListAsync();

            return merchantStores;
        }
    }
}