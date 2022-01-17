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
        /// Get Collection Include Resident By Collection Id
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public async Task<Collection> GetCollectionIncludeResidentByCollectionId(string collectionId)
        {
            Collection collection = await _context.Collections
                                            .Where(clt => clt.CollectionId.Equals(collectionId))
                                            .Include(clt => clt.Resident)
                                            .OrderByDescending(clt => clt.CreatedDate)
                                            .FirstOrDefaultAsync();

            return collection;
        }


        /// <summary>
        /// Get All Collections Include Resident
        /// </summary>
        /// <returns></returns>
        public async Task<List<Collection>> GetAllCollectionsIncludeResident()
        {
            List<Collection> collections = await _context.Collections
                                                    .Include(clt => clt.Resident)
                                                    .OrderByDescending(clt => clt.CreatedDate)
                                                    .ToListAsync();

            return collections;
        }
    }
}