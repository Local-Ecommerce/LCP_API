using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Collection> GetCollectionByCollectionId(string collectionId)
        {
            Collection collection = await _context.Collections
                                            .Where(clt => clt.CollectionId.Equals(collectionId))
                                            .OrderByDescending(clt => clt.CreatedDate)
                                            .FirstOrDefaultAsync();

            return collection;
        }


        /// <summary>
        /// Get All Collections Include Merchant
        /// </summary>
        /// <returns></returns>
        public async Task<List<Collection>> GetAllCollections()
        {
            List<Collection> collections = await _context.Collections.ToListAsync();

            return collections;
        }
    }
}