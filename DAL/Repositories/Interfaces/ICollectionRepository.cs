
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
        Task<Collection> GetCollectionIncludeMerchantByCollectionId(string collectionId);


        /// <summary>
        /// Get Collections Include Merchant By Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<List<Collection>> GetCollectionsIncludeMerchantByMerchantId(string merchantId);


        /// <summary>
        /// Get All Collections Include Merchant
        /// </summary>
        /// <returns></returns>
        Task<List<Collection>> GetAllCollectionsIncludeMerchant();
    }
}