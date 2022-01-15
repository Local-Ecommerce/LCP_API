
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        /// <summary>
        /// Get Collection Include Merchant By Collection Id
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        Task<Collection> GetCollectionByCollectionId(string collectionId);


        /// <summary>
        /// Get All Collections Include Merchant
        /// </summary>
        /// <returns></returns>
        Task<List<Collection>> GetAllCollections();
    }
}