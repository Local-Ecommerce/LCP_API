using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
        public CollectionRepository(LoichDBContext context) : base(context) { }

        /// <summary>
        /// Get Collection Include Merchant By Collection Id
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public async Task<Collection> GetCollectionIncludeMerchantByCollectionId(string collectionId)
        {
            Collection collection = await _context.Collections
                                            .Where(clt => clt.CollectionId.Equals(collectionId))
                                            .OrderByDescending(clt => clt.CreatedDate)
                                            .Include(clt => clt.Merchant).FirstOrDefaultAsync();

            return collection;
        }


        /// <summary>
        /// Get Collections Include Merchant By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<List<Collection>> GetCollectionsIncludeMerchantByMerchantId(string merchantId)
        {
            List<Collection> collections = await _context.Collections
                                            .Where(clt => clt.MerchantId.Equals(merchantId))
                                            .Include(clt => clt.Merchant).ToListAsync();

            return collections;
        }


        /// <summary>
        /// Get All Collections Include Merchant
        /// </summary>
        /// <returns></returns>
        public async Task<List<Collection>> GetAllCollectionsIncludeMerchant()
        {
            List<Collection> collections = await _context.Collections
                                            .Include(clt => clt.Merchant).ToListAsync();

            return collections;
        }
    }
}